using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chekPointScript : MonoBehaviour
{

    public Animator anim;
    public bool activate;


    void Start()
    {
        activate = false;
    }

    void Update()
    {
        if (activate == true) {
            anim.SetBool("activate", true);
            anim.SetBool("stay", false);
        }
    }

     void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            activate = true;
        }
    }
}
