using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    public float health;
    public gameController gameController;
    public float playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(target.transform.position);
    }

    public void takeDamage(float damage){
        health -= damage;
        if (health <= 0){
            Die();
        }
    }

    public void DealDamage(float damage){
        gameController.PlayerTakeDamage(damage);
    }

    void Die(){
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Player")){
            Debug.Log("Çarptı");
            DealDamage(10);
        }
    }
}
