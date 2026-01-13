using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (doorOpen == true) {
            Debug.Log("Enter the room");
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
