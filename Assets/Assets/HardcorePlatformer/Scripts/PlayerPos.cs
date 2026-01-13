using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPos : MonoBehaviour
{
    public bool toDie;

    private GameMaster gm;
    public ParticleSystem DeathEffect;
    public float deadTimmer;
    public float startDeadTimmer;
    public playerController pC;

    public void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        transform.position = gm.lastCheckPointPos;
        toDie = false;
        pC.enabled = true;
        pC.GetComponent<SpriteRenderer>().enabled = true;

    }

    // Update is called once per frame
    public void Update()
    {

        if (toDie == true)
        {
            pC.r2d.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
            
            deadTimmer -= Time.deltaTime;
            pC.enabled = false;
            pC.GetComponent<SpriteRenderer>().enabled = false;
            
            if (deadTimmer <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                gm.DamagePlayer(1);
            }



        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Nedles"))
        {
            toDie = true;
            Instantiate(DeathEffect, pC.groundCheck.transform.position, pC.groundCheck.transform.rotation);

        }
        if (other.CompareTag("Arrows"))
        {
            toDie = true;
            Instantiate(DeathEffect, pC.groundCheck.transform.position, pC.groundCheck.transform.rotation);
        }
        if (other.CompareTag("NeedlessOnGround")) {
            toDie = true;
            Instantiate(DeathEffect, pC.groundCheck.transform.position, pC.groundCheck.transform.rotation);
        }
        if (other.CompareTag("rock"))
        {
            toDie = true;
            Instantiate(DeathEffect, pC.groundCheck.transform.position, pC.groundCheck.transform.rotation);

        }

    }
    public void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Key")
        {
            gm.KeyTaked(1);
           
        }

    }

}
