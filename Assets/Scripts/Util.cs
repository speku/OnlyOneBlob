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
        sm.StartCoroutine(_Delay(start, delay, finish));
    }


    private static IEnumerator _Delay(Action start, float delay, Action finish = null)
    {
        start();
        yield return new WaitForSeconds(delay);
        if (finish != null) finish();
    }

    public static void While(Action action, Func<bool> predicate, float delay)
    {
        sm.StartCoroutine(_While(action, predicate, delay));
    }

    private static IEnumerator _While(Action action, Func<bool> predicate, float delay)
    {
        while (predicate())
        {
            action();
            yield return new WaitForSeconds(delay);
        }
    }

    public static bool LineOfSight(GameObject start, GameObject end)
    {
        RaycastHit hit;
        return !Physics.Linecast(start.transform.position, end.transform.position, out hit) || hit.collider.gameObject == end;
    }
}
