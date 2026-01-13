using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMaster : MonoBehaviour
{
    public static KeyMaster KM;
    public static GameMaster gm;
    public int TotalKeys;
    public int NeedKeys;

    public void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
    }
    public void Update()
    {
        if (gm.PlayerDeath == true) {
            Destroy(gameObject);
        }
    }
    void Awake()
    {
        if (KM == null)
        {
            KM = this;
            DontDestroyOnLoad(KM);


        }
        else
        {
            Destroy(gameObject);
        }
    }
}
