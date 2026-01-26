using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    public bool door;
    public bool doorReady;
    public bool doorOpened;
    public bool doorOpen;
    public GameMaster gm;
    public Animator anim;
    public float timeAnim = -1.08f;
    private float timeCD;
    public GameObject enterButton;
    
    [Header("Level Transition")]
    public float transitionDelay = 0.5f; // Delay before loading next level
    private bool isTransitioning = false;
    // Start is called before the first frame update
    void Start()
    {
        door = false;
        doorReady = false;
        doorOpened = false;
        doorOpen = false;
        enterButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.allKeysTaked == true) {
            door = true;
            anim.SetBool("takedAllKeys", true);
            if (door == true) {
                timeCD -= Time.deltaTime;
                if (timeCD <= timeAnim) {
                    anim.SetBool("blinkWithAllKeys", true);
                    anim.SetBool("takedAllKeys", false);
                    doorReady = true;
                    if (doorOpened == true) {
                        anim.SetBool("open", true);
                        anim.SetBool("blinkWithAllKeys", false);
                        doorOpen = true;
                    }
                }
            }
        }
        if (gm.PlayerDeath == true) {
            door = false;
            doorReady = false;
            doorOpened = false;
            doorOpen = false;
            Destroy(gameObject);
            gm.allKeysTaked = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && doorReady == true) {
            doorOpened = true;
            enterButton.gameObject.SetActive(true);
        }
        if (doorOpen == true && !isTransitioning) {
            Debug.Log("Enter the room - Loading next level...");
            StartCoroutine(TransitionToNextLevel());
        }
    }
    
    private IEnumerator TransitionToNextLevel()
    {
        isTransitioning = true;
        
        // Wait for transition delay (let door animation finish)
        yield return new WaitForSeconds(transitionDelay);
        
        // Use LevelManager if available, otherwise fallback to build index
        if (LevelManager.Instance != null)
        {
            // Check if this is the final level (win condition)
            bool isFinalLevel = LevelManager.Instance.currentLevel != null && 
                                LevelManager.Instance.currentLevel.nextLevel == null;
            
            if (isFinalLevel)
            {
                // Stop timer on win
                if (PlayTimeManager.Instance != null)
                {
                    PlayTimeManager.Instance.StopTimerOnWin();
                }
                Debug.Log("Congratulations! You completed the game!");
                // Optionally load a victory/credits scene here
            }
            
            // Save progress before loading next level
            if (LevelManager.Instance.currentLevel != null && LevelManager.Instance.currentLevel.nextLevel != null)
            {
                LevelManager.Instance.SaveProgress(LevelManager.Instance.currentLevel.nextLevel.levelIndex);
                
                // Save current play time for continuation
                if (PlayTimeManager.Instance != null)
                {
                    PlayTimeManager.Instance.SaveCurrentTime();
                }
            }
            LevelManager.Instance.LoadNextLevel();
        }
        else
        {
            // Fallback: load next scene by build index
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            
            // Save progress using build index as fallback
            PlayerPrefs.SetInt("HighestLevelReached", nextSceneIndex);
            PlayerPrefs.Save();
            
            // Check if this is the final level
            if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
            {
                // Stop timer on win
                if (PlayTimeManager.Instance != null)
                {
                    PlayTimeManager.Instance.StopTimerOnWin();
                }
                Debug.Log("Congratulations! You completed the game!");
            }
            else
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && doorReady == true)
        {
            
            enterButton.gameObject.SetActive(false);
        }
    }
}
