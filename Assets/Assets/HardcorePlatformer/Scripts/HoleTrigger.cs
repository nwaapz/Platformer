using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HoleTrigger : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public ParticleSystem DeathEffect;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Hide sprite at runtime, show only in editor
        if (spriteRenderer != null)
        {
            #if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                spriteRenderer.enabled = true;
            }
            else
            {
                spriteRenderer.enabled = false;
            }
            #else
            spriteRenderer.enabled = false;
            #endif
        }
    }

    void Start()
    {
        // Ensure sprite is hidden when game starts
        if (spriteRenderer != null && Application.isPlaying)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerController pC = collider.GetComponent<playerController>();
            if (pC != null)
            {
                // Find the main camera that is a child of the player
                Camera mainCamera = pC.GetComponentInChildren<Camera>();
                if (mainCamera != null)
                {
                    // Detach camera from player (move to root)
                    mainCamera.transform.SetParent(null);
                }
                
                // Disable player controls so they just fall
                pC.enabled = false;
                
                // Spawn death effect if assigned
                if (DeathEffect != null)
                {
                    Instantiate(DeathEffect, pC.groundCheck.transform.position, pC.groundCheck.transform.rotation);
                }
                
                // Start coroutine to reload scene after 1.5 seconds
                StartCoroutine(ReloadSceneAfterDelay(1.5f));
            }
        }
    }
    
    private IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        // Damage player and reload scene
        GameMaster gm = GameObject.FindGameObjectWithTag("GM")?.GetComponent<GameMaster>();
        if (gm != null)
        {
            gm.DamagePlayer(1);
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
