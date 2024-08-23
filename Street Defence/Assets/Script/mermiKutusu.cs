using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mermiKutusu : MonoBehaviour
{
        string [] silahlar = {"Magnum","Pompali","Sniper","Taramali"};
        int[] mermiSayısı = {10,20,30,50};
        public string olusan_silahin_turu;
        public int olusan_mermi_sayısı;
    // Start is called before the first frame update
    void Start()
    {
        // olusan_silahin_turu = silahlar[Random.Range(0, silahlar.Length - 1)];
        olusan_silahin_turu = "Taramali";
        olusan_mermi_sayısı = mermiSayısı[Random.Range(0, mermiSayısı.Length)];

        Debug.Log(olusan_silahin_turu);
        Debug.Log(olusan_mermi_sayısı);

    }

    // Update is called once per frame
    void Update()
    {
        // olusan_silahin_turu = silahlar[Random.Range(0,silahlar.Length - 1)];
         olusan_silahin_turu = "Taramali";
    }

}
