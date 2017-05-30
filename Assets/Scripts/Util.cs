using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Util {

    static SettingsManager sm;

    static Util()
    {
        sm = GameObject.FindObjectOfType<SettingsManager>();
    }

    public static void Delay(Action start, float delay, Action finish = null)
    {
        sm.StartCoroutine(Execute(start, delay, finish));
    }

    public static IEnumerator Execute(Action start, float delay, Action finish = null)
    {
        start();
        yield return new WaitForSeconds(delay);
        if (finish != null) finish();
    }
	
}
