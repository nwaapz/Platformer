using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class go_scrip : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Restart() {
        if (LevelManager.Instance != null) {
            LevelManager.Instance.ReloadCurrentLevel();
        } else {
            SceneManager.LoadScene(2); // Fallback
        }
    }
    public void exitMenu() {
        SceneManager.LoadScene(0); // Menu is always at index 0
    }
}
