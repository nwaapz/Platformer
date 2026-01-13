using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTrap : MonoBehaviour
{
    public Transform shoot_point;
    public GameObject Bullet;
    public float TimeShoot;
    public float StartTimeShoot;
    public Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (TimeShoot <= 0)
        {
            Instantiate(Bullet, shoot_point.position, transform.rotation);
            TimeShoot = StartTimeShoot;
            anim.SetBool("fire", true);
            anim.SetBool("ready", false);
            
        }
        else
        {
            TimeShoot -= Time.deltaTime;
            if (Time.deltaTime > 0) {
                anim.SetBool("fire", false);
                anim.SetBool("ready", true);
            }
        }
 
    }
}
