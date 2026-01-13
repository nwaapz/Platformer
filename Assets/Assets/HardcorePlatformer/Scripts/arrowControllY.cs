using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowControllY : MonoBehaviour
{

    public int speed;
    public GameObject DestroydEffect;
    public GameObject arrow_destroy;
    public SpriteRenderer arrowDestrSp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.up * speed * Time.deltaTime);
        if (speed > 0)
        {
            arrowDestrSp.flipY = false;
        }
        if (speed < 0) {
            arrowDestrSp.flipY = true;
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Ground"))
        {
            Destroy(gameObject);
            Instantiate(arrow_destroy, transform.position, transform.rotation);
        }
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            Instantiate(arrow_destroy, transform.position, transform.rotation);
        }

    }

}
