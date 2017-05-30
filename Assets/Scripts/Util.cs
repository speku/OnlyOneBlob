using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Util {

    static SettingsManager sm;

    static ContactFilter2D filterLineOfSight;

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
        return Physics2D.Linecast(start.transform.position, end.transform.position, LayerMask.NameToLayer("Obstacle")).collider == null;
    }

    public static void IgnoreCollisions(string layer1, string layer2)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(layer1), LayerMask.NameToLayer(layer2), true);
    }

    public static void Fade(SpriteRenderer sr, float speed)
    {
        While(() => Alpha(sr, sr.color.a - speed * Time.deltaTime), () => sr.color.a > 0, 0.05f);
    }

    public static void Alpha(SpriteRenderer sr, float alpha)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }
}
