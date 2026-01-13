using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowControlX : MonoBehaviour
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
        transform.Translate(transform.right * speed * Time.deltaTime);
        if (speed > 0)
        {
            arrow_destroy.transform.Rotate(0, 0, 90);
        }
        if (speed < 0)
        {
            arrow_destroy.transform.Rotate(0, 0, -90);
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
