using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeedlesOnGround : MonoBehaviour
{
    public Animator anim;
    public float CD;
    public float CDready;
    public float CDpool;
    public float timeAnimReady;
    public float timeAnimFall;
    public float timeAnimPool;
    public Collider2D readyCol;
    public bool stay;
    public bool ready;
    public bool pool;
    public bool fall;
    public bool timmer;
    // Start is called before the first frame update
    void Start()
    {
        readyCol.GetComponent<Collider2D>().enabled = false;
        stay = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (timmer == true) {
            CD -= Time.deltaTime;
        }
        if (timmer == false) {
            CD = 3.5f;
        }

        if (ready == true) {
            stay = false;
            anim.SetBool("ready", true);
            anim.SetBool("stay", false);
            


        }
        if (CD <= timeAnimReady)
        {
            
            ready = false;
            pool = true;
            if (pool == true)
            {
                anim.SetBool("pool", true);
                anim.SetBool("ready", false);
                readyCol.GetComponent<Collider2D>().enabled = true;
                if (CD <= timeAnimPool)
                {
                    CDpool = 1;
                    pool = false;
                    fall = true;
                    if (fall == true) {
                        anim.SetBool("pool", false);
                        anim.SetBool("fall", true);
                        readyCol.GetComponent<Collider2D>().enabled = false;
                        if (CD <= timeAnimFall)
                        {
                            fall = false;
                            stay = true;
                            if(stay == true) {
                                anim.SetBool("fall", false);
                                anim.SetBool("stay", true);
                                timmer = false;
                                
                            }


                        }
                    }
                }
            }


            
        }

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") {
            ready = true;
            timmer = true;

        }
        
    }
}
