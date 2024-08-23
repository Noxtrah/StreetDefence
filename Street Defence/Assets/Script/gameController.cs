using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject[] enemies;
    public GameObject[] spawnPoints;
    [Header("Health Settings")]
    public float playerHealth;
    public UnityEngine.UI.Image HealthBar;
    private RectTransform healthBarRectTransform;
    private float maxWidth;
    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 100;
        // Get the RectTransform component from the Image
        healthBarRectTransform = HealthBar.GetComponent<RectTransform>();

         // Set the anchor to the left (anchorMin and anchorMax at 0)
        healthBarRectTransform.anchorMin = new Vector2(0, 0.5f);
        healthBarRectTransform.anchorMax = new Vector2(0, 0.5f);

        // Set pivot to left (0, 0.5)
        healthBarRectTransform.pivot = new Vector2(0, 0.5f);

        // Store the initial width of the health bar
        maxWidth = healthBarRectTransform.sizeDelta.x;

        // Example call to UpdateHealthBar in Start with 75% health
        UpdateHealthBar();

        if(!PlayerPrefs.HasKey("OyunBasladiMi")){
            PlayerPrefs.SetInt("Taramali_Toplam_Mermi", 170);
            PlayerPrefs.SetInt("Taramali_Kalan_Mermi",25);
            PlayerPrefs.SetInt("OyunBasladiMi",1);
        }
        StartCoroutine(SpawnEnemy());
    }

    // void Update(){
    //     Debug.Log("Player health: " + playerHealth);
    //     if (playerHealth > 0)
    //     {
    //         // playerHealth -= Time.deltaTime * 10;  // Decrease health by 10 units per second
    //         UpdateHealthBar();  // Update the health bar every frame
    //     }
    // }

    public void PlayerTakeDamage(float damage)
    {
        playerHealth -= damage;

        if (playerHealth < 0)
            playerHealth = 0;

        // Update the health bar with animation
        StartCoroutine(AnimateHealthBar(0.5f)); // Adjust the duration as needed
    }

    IEnumerator SpawnEnemy() {
        yield return new WaitForSeconds(2f);

         while(true)
        {
            int numberOfEnemies = Random.Range(0, enemies.Length);
            int numberOfSpawnPoints = Random.Range(0, spawnPoints.Length);

            Instantiate(enemies[numberOfEnemies], spawnPoints[numberOfSpawnPoints].transform.position, Quaternion.identity);

            // Add a delay to control the spawn rate
            yield return new WaitForSeconds(3f); // Adjust the delay as needed
        }
    }

    public void UpdateHealthBar()
    {
        // Adjust the width based on the health percentage
        StartCoroutine(AnimateHealthBar(0.5f));
    }

     private IEnumerator AnimateHealthBar(float duration)
    {
        float targetHealthPercentage = playerHealth / 100;
        float elapsedTime = 0f;
        float startWidth = healthBarRectTransform.sizeDelta.x;
        float targetWidth = maxWidth * targetHealthPercentage;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newWidth = Mathf.Lerp(startWidth, targetWidth, elapsedTime / duration);
            healthBarRectTransform.sizeDelta = new Vector2(newWidth, healthBarRectTransform.sizeDelta.y);
            yield return null;
        }

        // Ensure the final width is exactly the target width
        healthBarRectTransform.sizeDelta = new Vector2(targetWidth, healthBarRectTransform.sizeDelta.y);
    }
}
