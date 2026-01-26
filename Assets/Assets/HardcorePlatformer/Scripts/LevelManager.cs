using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    [Header("Current Level")]
    public LevelData currentLevel;
    
    [Header("Level Database")]
    public LevelData[] allLevels;
    
    private const string HIGHEST_LEVEL_KEY = "HighestLevelReached";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    /// <summary>
    /// Get the highest level index the player has reached
    /// </summary>
    public int GetHighestLevelReached()
    {
        return PlayerPrefs.GetInt(HIGHEST_LEVEL_KEY, 0);
    }
    
    /// <summary>
    /// Save progress when reaching a new level
    /// </summary>
    public void SaveProgress(int levelIndex)
    {
        int currentHighest = GetHighestLevelReached();
        if (levelIndex > currentHighest)
        {
            PlayerPrefs.SetInt(HIGHEST_LEVEL_KEY, levelIndex);
            PlayerPrefs.Save();
            Debug.Log($"Progress saved! Highest level: {levelIndex}");
        }
    }
    
    /// <summary>
    /// Continue from the highest level reached
    /// </summary>
    public void ContinueGame()
    {
        int highestLevel = GetHighestLevelReached();
        
        // Load and resume play time tracker
        if (PlayTimeManager.Instance != null)
        {
            PlayTimeManager.Instance.LoadSavedTime();
            PlayTimeManager.Instance.StartTimer();
        }
        
        LoadLevelByIndex(highestLevel);
    }
    
    /// <summary>
    /// Start a new game from level 0
    /// </summary>
    public void StartNewGame()
    {
        // Reset and start play time tracker
        if (PlayTimeManager.Instance != null)
        {
            PlayTimeManager.Instance.ResetTimer();
            PlayTimeManager.Instance.StartTimer();
        }
        LoadLevelByIndex(0);
    }
    
    /// <summary>
    /// Reset all saved progress
    /// </summary>
    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey(HIGHEST_LEVEL_KEY);
        PlayerPrefs.Save();
        
        // Also reset play time
        if (PlayTimeManager.Instance != null)
        {
            PlayTimeManager.Instance.ResetTimer();
        }
        
        Debug.Log("Progress reset!");
    }
    
    /// <summary>
    /// Check if there is saved progress to continue
    /// </summary>
    public bool HasSavedProgress()
    {
        return PlayerPrefs.HasKey(HIGHEST_LEVEL_KEY) && GetHighestLevelReached() > 0;
    }
    
    /// <summary>
    /// Load the next level based on currentLevel's nextLevel reference
    /// </summary>
    public void LoadNextLevel()
    {
        if (currentLevel != null && currentLevel.nextLevel != null)
        {
            currentLevel = currentLevel.nextLevel;
            SceneManager.LoadScene(currentLevel.sceneName);
        }
        else
        {
            Debug.LogWarning("No next level assigned!");
            // Optionally load a credits or main menu scene
            // SceneManager.LoadScene("MainMenu");
        }
    }
    
    /// <summary>
    /// Load a specific level by LevelData
    /// </summary>
    public void LoadLevel(LevelData level)
    {
        if (level != null)
        {
            currentLevel = level;
            SceneManager.LoadScene(level.sceneName);
        }
    }
    
    /// <summary>
    /// Load a level by its index in allLevels array
    /// </summary>
    public void LoadLevelByIndex(int index)
    {
        if (index >= 0 && index < allLevels.Length)
        {
            LoadLevel(allLevels[index]);
        }
        else
        {
            Debug.LogError($"Level index {index} out of range!");
        }
    }
    
    /// <summary>
    /// Reload the current level
    /// </summary>
    public void ReloadCurrentLevel()
    {
        if (currentLevel != null)
        {
            SceneManager.LoadScene(currentLevel.sceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    
    /// <summary>
    /// Get the keys required for current level
    /// </summary>
    public int GetKeysRequired()
    {
        return currentLevel != null ? currentLevel.keysRequired : 0;
    }
}
