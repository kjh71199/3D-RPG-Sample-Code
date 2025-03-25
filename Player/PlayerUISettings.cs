using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUISettings : MonoBehaviour
{
    private static PlayerUISettings instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
}
