using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bosKovan : MonoBehaviour
{

    AudioSource yereDusmeSesi;
    // Start is called before the first frame update
    void Start()
    {
        yereDusmeSesi = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision){

        if(collision.gameObject.CompareTag("Untagged")){
            yereDusmeSesi.Play();

            if(!yereDusmeSesi.isPlaying){
                Destroy(gameObject,45f);
            }

            Debug.Log("Çarptı");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
