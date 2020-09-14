using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyScript : MonoBehaviour {

    public static DontDestroyScript Instance;

    void Awake()
    {
            DontDestroyOnLoad(gameObject);
    }
}
