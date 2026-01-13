using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public static UIScript UI;
    // Start is called before the first frame update
    void Awake()
    {
        if (UI == null)
        {
            UI = this;
            DontDestroyOnLoad(UI);


        }
        else
        {
            Destroy(gameObject);
        }
    }
}
