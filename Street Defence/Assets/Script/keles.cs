using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class keles : MonoBehaviour
{
    public CreateAmmoCrate createAmmoCrateScript;
    [Header("Değişkenler")]
    public bool atesEdebilirmi;
    float içeridenAtesEtmeSikligi;
    public float disaridanAtesEtmeSikligi;
    public float menzil;
    public Camera benimCam;

    [Header("Sesler")]
    public AudioSource atesSesi;
    public AudioSource reloadSound;
    public AudioSource emptyMagazinSound;
    public AudioSource mermiAlmaSesi;

    [Header("Efektler")]
    public ParticleSystem atesEfekt;
    public GameObject mermiİzi;
    public GameObject kanEfekti;

    [Header("Animasyonlar")]
    Animator fireAnimation;
    Animator reloadAnimation;
    bool isReloading = false;

    [Header("Silah Ayarları")]
    int ToplamMermiSayısı;
    public int ŞarjörKapasitesi;
    int KalanMermi;
    public TextMeshProUGUI ToplamMermiText;
    public TextMeshProUGUI KalanMermiText;
    public float damage;

    [Header("Kovan Ayarları")]
    public bool KovanCıksınMı;
    public GameObject KovanObjesi;
    public GameObject KovanCıkışNoktası;

    // Start is called before the first frame update
    void Start()
    {
        ToplamMermiSayısı = PlayerPrefs.GetInt("Taramali_Toplam_Mermi");
        ToplamMermiText.text = "/" + ToplamMermiSayısı.ToString();
        KalanMermi = PlayerPrefs.GetInt("Taramali_Kalan_Mermi");
        KalanMermiText.text = KalanMermi.ToString();

        fireAnimation = GetComponent<Animator>();
        reloadAnimation = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse0)){

            if(atesEdebilirmi && Time.time > içeridenAtesEtmeSikligi && KalanMermi > 0){
                AtesEt();
                içeridenAtesEtmeSikligi = Time.time + disaridanAtesEtmeSikligi;
            }
            else if (KalanMermi == 0 && !emptyMagazinSound.isPlaying){
                emptyMagazinSound.Play();
            }

        }

        if(Input.GetKey(KeyCode.R)){

            if(KalanMermi != ŞarjörKapasitesi){
                Reload();
            }
        }

        if(Input.GetKeyDown(KeyCode.E)){
            MermiAl();
        }
    }

    void Reload()
    {
        reloadAnimation.Play("Reload");
        if (!isReloading)
        {
            StartCoroutine(ReoladCoroutine());
        }

    }

    IEnumerator ReoladCoroutine()
    {
        if(ToplamMermiSayısı > 0){

            isReloading = true;
            yield return new WaitForSeconds(0.7f);
            reloadSound.Play();
            yield return new WaitForSeconds(reloadSound.clip.length + 0.25f);
            reloadSound.Play();
            ReloadMathFunction();

        }

        isReloading = false;

    }

    void ReloadMathFunction(){
        int DoldurulabilirMermiSayısı = ŞarjörKapasitesi - KalanMermi;

            if(ToplamMermiSayısı < DoldurulabilirMermiSayısı){
                DoldurulabilirMermiSayısı = ToplamMermiSayısı;
                Debug.Log("Doldurulabilir Mermi Sayızı: " + DoldurulabilirMermiSayısı);
                KalanMermi += DoldurulabilirMermiSayısı ;
                PlayerPrefs.SetInt("Taramali_Kalan_Mermi", KalanMermi);

                Debug.Log("Kalan Mermi Sayızı: " + KalanMermi);
                ToplamMermiSayısı -= DoldurulabilirMermiSayısı;
                PlayerPrefs.SetInt("Taramali_Toplam_Mermi", ToplamMermiSayısı);
                Debug.Log("Toplam Mermi Sayızı: " + ToplamMermiSayısı);

                KalanMermiText.text = KalanMermi.ToString();
                ToplamMermiText.text = "/" + ToplamMermiSayısı.ToString();
            }

            
            else {
                KalanMermi += DoldurulabilirMermiSayısı;
                PlayerPrefs.SetInt("Taramali_Kalan_Mermi", KalanMermi);
                ToplamMermiSayısı -= DoldurulabilirMermiSayısı;
                PlayerPrefs.SetInt("Taramali_Toplam_Mermi", ToplamMermiSayısı);

                KalanMermiText.text = KalanMermi.ToString();
                ToplamMermiText.text = "/" + ToplamMermiSayısı.ToString();
            }
    }

    void AtesEt(){

        RaycastHit hit;

        if(KovanCıksınMı){
            GameObject obje = Instantiate(KovanObjesi, KovanCıkışNoktası.transform.position, KovanCıkışNoktası.transform.rotation);
            Rigidbody rb = obje.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(-10f,1,-0) * 15);
        }

        atesSesi.Play();
        atesEfekt.Play();
        fireAnimation.Play("Fire");

        if(Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, menzil)){

            if(hit.transform.gameObject.CompareTag("Enemy")){
                Instantiate(kanEfekti, hit.point, Quaternion.LookRotation(hit.normal));

                hit.transform.gameObject.GetComponent<Enemy>().takeDamage(damage);
            }
            else if(hit.transform.gameObject.CompareTag("Toppleable Object")){
                Rigidbody rg = hit.transform.gameObject.GetComponent<Rigidbody>();
                // rg.AddForce(transform.forward * 400f);
                rg.AddForce(-hit.normal * 50f);
            }
            else{
                Instantiate(mermiİzi, hit.point, Quaternion.LookRotation(hit.normal));
            }
        }

        KalanMermi--;
        PlayerPrefs.SetInt("Taramali_Kalan_Mermi", KalanMermi);
        KalanMermiText.text = KalanMermi.ToString();

    }

    void MermiAl(){
        RaycastHit hit;

        if(Physics.Raycast(benimCam.transform.position, benimCam.transform.forward, out hit, 4)){

            if(hit.transform.gameObject.CompareTag("Mermi")){
                // ToplamMermiSayısı += hit.transform.gameObject.GetComponent<mermiKutusu>().olusan_mermi_sayısı;
                // ToplamMermiText.text = "/" + ToplamMermiSayısı.ToString();
                mermiAlmaSesi.Play();
                Debug.Log("Kutuda oluşan silahın türü: " + hit.transform.gameObject.GetComponent<mermiKutusu>().olusan_silahin_turu);

                MermiKaydet(hit.transform.gameObject.GetComponent<mermiKutusu>().olusan_silahin_turu, hit.transform.gameObject.GetComponent<mermiKutusu>().olusan_mermi_sayısı);
                int index = -1;
                float closestDistance = Mathf.Infinity;
                for (int i = 0; i < createAmmoCrateScript.MermiKutusuPoint.Count; i++)
                {
                    float distance = Vector3.Distance(hit.transform.position, createAmmoCrateScript.MermiKutusuPoint[i].transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        index = i;
                    }
                }

                // Mark the position as empty again
                if (index != -1)
                {
                    createAmmoCrateScript.isEmpty[index] = true;
                }
                Destroy(hit.transform.gameObject);
            }
        }
    }

    void MermiKaydet(string silahTuru, int mermiSayisi){
        Debug.Log("Fonksiyona girdi");
        switch(silahTuru){
            case "Taramali":
                ToplamMermiSayısı += mermiSayisi;
                Debug.Log("Mermi aldıktan sonraki alınan mermi sayısı: " + mermiSayisi);
                Debug.Log("Mermi aldıktan sonraki toplam mermi sayısı: " + ToplamMermiSayısı);
                PlayerPrefs.SetInt("Taramali_Toplam_Mermi", ToplamMermiSayısı);
                ToplamMermiText.text = "/" + ToplamMermiSayısı.ToString(); 
                break;
            case "Pompali":
                break;
            case "Magnum":
                break;
            case "Sniper":
                break;
        }
    }
}
