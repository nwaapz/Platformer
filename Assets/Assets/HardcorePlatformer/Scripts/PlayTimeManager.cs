using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Persistent manager that tracks total play time across scenes.
/// Timer runs while playing and stops when player wins (completes final level).
/// </summary>
public class PlayTimeManager : MonoBehaviour
{
    public static PlayTimeManager Instance { get; private set; }
    
    [Header("Timer Settings")]
    [SerializeField] private int finalLevelIndex = 9; // Level 10 is index 9 (0-based)
    
    [Header("UI Reference (Optional)")]
    [SerializeField] private Text timerText; // Assign in inspector or find by tag
    [SerializeField] private string timerTextTag = "TimerText"; // Tag to find timer text if not assigned
    
    // Timer state
    private float totalPlayTime = 0f;
    private bool isTimerRunning = false;
    private bool hasWon = false;
    
    // PlayerPrefs keys
    private const string BEST_TIME_KEY = "BestPlayTime";
    private const string CURRENT_TIME_KEY = "CurrentPlayTime";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Subscribe to scene loaded event to find UI elements
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // Find timer text on first load
        FindTimerText();
        
        // Auto-start if we're in a gameplay scene
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex >= 2 && !hasWon && !isTimerRunning)
        {
            Debug.Log($"PlayTimeManager: Auto-starting timer on Start in scene index {sceneIndex}");
            isTimerRunning = true;
        }
        
        // Update display with initial value
        UpdateTimerDisplay();
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Try to find timer text in new scene if not assigned
        FindTimerText();
        
        // Auto-start timer if we're in a gameplay scene (not main menu)
        // Main menu is typically scene index 0 or 1
        int sceneIndex = scene.buildIndex;
        if (sceneIndex >= 2 && !hasWon)
        {
            // If timer was not running (e.g., launched directly into level), start it
            if (!isTimerRunning)
            {
                Debug.Log($"PlayTimeManager: Auto-starting timer in scene {scene.name}");
                isTimerRunning = true;
            }
        }
    }
    
    private void FindTimerText()
    {
        // First try to find by assigned reference
        if (timerText != null) return;
        
        // Try to find by tag
        if (!string.IsNullOrEmpty(timerTextTag))
        {
            GameObject timerObj = GameObject.FindGameObjectWithTag(timerTextTag);
            if (timerObj != null)
            {
                timerText = timerObj.GetComponent<Text>();
                if (timerText != null)
                {
                    Debug.Log($"PlayTimeManager: Found timer text by tag '{timerTextTag}'");
                    return;
                }
            }
        }
        
        // Try to find by name (fallback)
        GameObject timerByName = GameObject.Find("TimerText");
        if (timerByName == null) timerByName = GameObject.Find("Timer");
        if (timerByName == null) timerByName = GameObject.Find("TimeText");
        if (timerByName == null) timerByName = GameObject.Find("Text (Legacy)"); // Common Unity name
        
        if (timerByName != null)
        {
            timerText = timerByName.GetComponent<Text>();
            if (timerText != null)
            {
                Debug.Log($"PlayTimeManager: Found timer text by name '{timerByName.name}'");
            }
        }
    }
    
    private void Update()
    {
        if (isTimerRunning && !hasWon)
        {
            totalPlayTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }
    
    private void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            timerText.text = GetFormattedTime();
        }
    }
    
    /// <summary>
    /// Get the current play time formatted as MM:SS:MS or HH:MM:SS:MS
    /// </summary>
    public string GetFormattedTime()
    {
        int hours = (int)(totalPlayTime / 3600);
        int minutes = (int)((totalPlayTime % 3600) / 60);
        int seconds = (int)(totalPlayTime % 60);
        int milliseconds = (int)((totalPlayTime * 100) % 100); // 2 digits for centiseconds
        
        if (hours > 0)
        {
            return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, milliseconds);
        }
        else
        {
            return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }
    
    /// <summary>
    /// Get raw play time in seconds
    /// </summary>
    public float GetPlayTime()
    {
        return totalPlayTime;
    }
    
    /// <summary>
    /// Start or resume the timer
    /// </summary>
    public void StartTimer()
    {
        if (!hasWon)
        {
            isTimerRunning = true;
            Debug.Log($"PlayTimeManager: Timer started at {GetFormattedTime()}");
        }
    }
    
    /// <summary>
    /// Pause the timer and freeze gameplay (e.g., for pause menu)
    /// </summary>
    public void PauseTimer()
    {
        isTimerRunning = false;
        Time.timeScale = 0f; // Freeze gameplay
        Debug.Log($"PlayTimeManager: Timer paused and game frozen at {GetFormattedTime()}");
    }
    
    /// <summary>
    /// Resume the timer and unfreeze gameplay after pause
    /// </summary>
    public void ResumeTimer()
    {
        if (!hasWon)
        {
            isTimerRunning = true;
            Time.timeScale = 1f; // Unfreeze gameplay
            Debug.Log($"PlayTimeManager: Timer resumed and game unfrozen at {GetFormattedTime()}");
        }
    }
    
    /// <summary>
    /// Reset the timer for a new game
    /// </summary>
    public void ResetTimer()
    {
        totalPlayTime = 0f;
        hasWon = false;
        isTimerRunning = false;
        PlayerPrefs.DeleteKey(CURRENT_TIME_KEY);
        Debug.Log("PlayTimeManager: Timer reset");
        UpdateTimerDisplay();
    }
    
    /// <summary>
    /// Stop the timer when player wins
    /// </summary>
    public void StopTimerOnWin()
    {
        if (hasWon) return; // Prevent double-stopping
        
        isTimerRunning = false;
        hasWon = true;
        
        Debug.Log($"PlayTimeManager: Player won! Final time: {GetFormattedTime()}");
        
        // Save best time if this is better
        SaveBestTime();
    }
    
    /// <summary>
    /// Save current time for continuation
    /// </summary>
    public void SaveCurrentTime()
    {
        PlayerPrefs.SetFloat(CURRENT_TIME_KEY, totalPlayTime);
        PlayerPrefs.Save();
    }
    
    /// <summary>
    /// Load saved time for continuation
    /// </summary>
    public void LoadSavedTime()
    {
        totalPlayTime = PlayerPrefs.GetFloat(CURRENT_TIME_KEY, 0f);
        UpdateTimerDisplay();
    }
    
    /// <summary>
    /// Save best time if current time is better
    /// </summary>
    private void SaveBestTime()
    {
        float bestTime = PlayerPrefs.GetFloat(BEST_TIME_KEY, float.MaxValue);
        
        if (totalPlayTime < bestTime)
        {
            PlayerPrefs.SetFloat(BEST_TIME_KEY, totalPlayTime);
            PlayerPrefs.Save();
            Debug.Log($"PlayTimeManager: New best time! {GetFormattedTime()}");
        }
    }
    
    /// <summary>
    /// Get the best recorded time
    /// </summary>
    public float GetBestTime()
    {
        return PlayerPrefs.GetFloat(BEST_TIME_KEY, 0f);
    }
    
    /// <summary>
    /// Get formatted best time string
    /// </summary>
    public string GetFormattedBestTime()
    {
        float best = GetBestTime();
        if (best <= 0f) return "--:--:--";
        
        int hours = (int)(best / 3600);
        int minutes = (int)((best % 3600) / 60);
        int seconds = (int)(best % 60);
        int milliseconds = (int)((best * 100) % 100);
        
        if (hours > 0)
        {
            return string.Format("{0:00}:{1:00}:{2:00}:{3:00}", hours, minutes, seconds, milliseconds);
        }
        else
        {
            return string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }
    
    /// <summary>
    /// Check if the given level index is the final level
    /// </summary>
    public bool IsFinalLevel(int levelIndex)
    {
        return levelIndex >= finalLevelIndex;
    }
    
    /// <summary>
    /// Check if player has won
    /// </summary>
    public bool HasPlayerWon()
    {
        return hasWon;
    }
    
    /// <summary>
    /// Assign timer text reference manually
    /// </summary>
    public void SetTimerText(Text text)
    {
        timerText = text;
        UpdateTimerDisplay();
    }
}
