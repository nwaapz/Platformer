using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platformMove : MonoBehaviour
{
    public bool UpDown;
    public bool LeftRight;
    public bool UP;
    public bool DOWN;
    public bool LEFT;
    public bool RIGHT;
  

    public float speed;

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (UP == true) {
            transform.Translate(transform.up * speed * Time.deltaTime);

        }
        if (DOWN == true) {
            transform.Translate(transform.up * -speed * Time.deltaTime);
        }
        if (RIGHT == true) {
            transform.Translate(transform.right * speed * Time.deltaTime);
        }
        if (LEFT == true) {
            transform.Translate(transform.right * -speed * Time.deltaTime);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (UpDown == true)
        {
            if (other.CompareTag("Platform_UP"))
            {
                DOWN = true;
                UP = false;

            }
            if (other.CompareTag("Platform_DOWN"))
            {
                UP = true;
                DOWN = false;
            }
        }
        if (LeftRight == true)
        {

                if (other.CompareTag("Platform_LEFT"))
                {
                RIGHT = true;
                LEFT = false;
                }
                if (other.CompareTag("Platform_RIGHT"))
                {
                RIGHT = false;
                LEFT = true;
                }
            
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            collision.collider.transform.SetParent(null);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            collision.collider.transform.SetParent(transform);
        }
    }



}
