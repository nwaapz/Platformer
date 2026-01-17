using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mMscript : MonoBehaviour
{
    [Header("Continue Button (Optional)")]
    public Button continueButton; // Assign in inspector if you have a continue button
    
    void Start()
    {
        // Disable continue button if no saved progress
        if (continueButton != null)
        {
            bool hasSave = LevelManager.Instance != null && LevelManager.Instance.HasSavedProgress();
            continueButton.interactable = hasSave;
        }
    }

    void Update()
    {
        
    }
    
    public void StartGame() {
        if (LevelManager.Instance != null) {
            LevelManager.Instance.LoadLevelByIndex(0); // Load first level from LevelManager
        } else {
            SceneManager.LoadScene(2); // Fallback to build index
        }
    }
    
    public void ContinueGame() {
        if (LevelManager.Instance != null && LevelManager.Instance.HasSavedProgress()) {
            LevelManager.Instance.ContinueGame();
        } else {
            // No saved progress, start from beginning
            StartGame();
        }
    }
    
    public void Exit() {
        Application.Quit();
    }
}
