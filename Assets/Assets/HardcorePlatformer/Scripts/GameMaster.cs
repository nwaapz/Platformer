using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public Vector2 lastCheckPointPos;
    public Text KeyText;
    public Text HealthText;
    public Image doorImage;
    public float timmer;
    public float cdtimmer;
    public bool PlayerDeath;


    private PlayerPos pp;

    [System.Serializable]
    public class PlayerStats
    {
        public int Health = 9;
    }
    public PlayerStats playerStats = new PlayerStats();
    public bool allKeysTaked;
    public static KeyMaster KM;


    public void Start()
    {
        allKeysTaked = false;
        doorImage.enabled = false;
        PlayerDeath = false;
        KM = GameObject.FindGameObjectWithTag("KM").GetComponent<KeyMaster>();

    }
    public void Update()
    {
        KeyText.text = KM.TotalKeys.ToString() + " / " + KM.NeedKeys.ToString();
        HealthText.text = " x " + playerStats.Health.ToString();
        
        if (KM.TotalKeys == KM.NeedKeys) {
            Debug.Log("all Keys taked");
            allKeysTaked = true;
            doorImage.enabled = true;
            if (doorImage.enabled == true) {
                timmer -= Time.deltaTime;
                if (timmer <= cdtimmer) {
                    doorImage.enabled = false;
                }
            }
        }
        if (playerStats.Health == 0) {
            KM.TotalKeys = 0;  // Reset key count on death
            
            // Reset timer on game over
            if (PlayTimeManager.Instance != null)
            {
                PlayTimeManager.Instance.ResetTimer();
            }
            
            SceneManager.LoadScene(1);
            Destroy(gameObject);
            PlayerDeath = true;
            allKeysTaked = false;
        }
    }

    void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            

        }
        else {
            Destroy(gameObject);
        }
    }

    public void DamagePlayer(int damage)
    {
        playerStats.Health -= damage;
    }
    public void KeyTaked(int key) {
        KM.TotalKeys += key;
    }
}
