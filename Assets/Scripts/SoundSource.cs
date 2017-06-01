using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour {

    static bool initialized = false;
    static GameObject o;

    private void Awake()
    {
        if (!initialized)
        {
            initialized = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}
