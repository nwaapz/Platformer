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
            // Use the same death mechanism as other hazards
            PlayerPos playerPos = collider.GetComponent<PlayerPos>();
            if (playerPos != null)
            {
                playerPos.toDie = true;
                
                // Spawn death effect if assigned
                if (DeathEffect != null && playerPos.pC != null)
                {
                    Instantiate(DeathEffect, playerPos.pC.groundCheck.transform.position, playerPos.pC.groundCheck.transform.rotation);
                }
            }
        }
    }
}
