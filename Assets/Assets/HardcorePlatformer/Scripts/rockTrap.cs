using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rockTrap : MonoBehaviour
{
    public int speed;
    public float CD;
    public float timmer;
    public rockTrigger tr;
    public GameObject rockDestr;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (tr.pool == true)
        {
            CD -= Time.deltaTime;
            if (CD <= timmer)
                transform.Translate(transform.up * -speed * Time.deltaTime);
        }
    }
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "RockDestr")
        {

            Destroy(gameObject);
            Instantiate(rockDestr, transform.position, transform.rotation);
        }

    }

}
