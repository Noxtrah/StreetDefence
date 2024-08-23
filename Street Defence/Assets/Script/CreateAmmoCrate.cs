using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAmmoCrate : MonoBehaviour
{
    public List<GameObject> MermiKutusuPoint = new List<GameObject>();
    public GameObject MermiKutusu;
    public bool []isEmpty;
    // Start is called before the first frame update
    void Start()
    {
        isEmpty = new bool[MermiKutusuPoint.Count]; // Initialize isEmpty array
        for (int i = 0; i < isEmpty.Length; i++)
        {
            isEmpty[i] = true; // Set all positions as empty at the start
        }
        StartCoroutine(CreateAmmoCrateFunc());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CreateAmmoCrateFunc(){

        while(true){
            yield return new WaitForSeconds(5f);
            int randomNumber = Random.Range(0,6);
            if(isEmpty[randomNumber]){
                Instantiate(MermiKutusu, MermiKutusuPoint[randomNumber].transform.position, MermiKutusuPoint[randomNumber].transform.rotation);
                isEmpty[randomNumber] = false;
                Debug.Log("Dolan alan : " + randomNumber + "    " + isEmpty[randomNumber]);
            }
        }
    }
}
