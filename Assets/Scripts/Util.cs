using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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
        if (start != null) start();
        yield return new WaitForSeconds(delay);
        if (finish != null) finish();
    }

    public static void While(Func<bool> predicate, Action action, float delay, Action finish = null)
    {
        sm.StartCoroutine(_While(predicate, action, delay, finish));
    }

    private static IEnumerator _While(Func<bool> predicate, Action action, float delay, Action finish)
    {
        while (predicate())
        {
            action();
            yield return new WaitForSeconds(delay);
        }
        if (finish != null) finish();
    }

    public static bool LineOfSight(GameObject start, GameObject end)
    {
        return true;
        //if (start == null || end == null) return false;
        //return Physics2D.Linecast(start.transform.position, end.transform.position, LayerMask.NameToLayer("Obstacle")).collider == null;
    }

    public static void IgnoreCollisions(string layer1, string layer2)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(layer1), LayerMask.NameToLayer(layer2), true);
    }

    public static void Fade(SpriteRenderer sr, float speed)
    {
        if (sr == null) return;
        While(() => sr.color.a > 0, () => Alpha(sr, sr.color.a - speed * Time.deltaTime), 0.05f);
    }

    public static void Alpha(SpriteRenderer sr, float alpha)
    {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
    }

    public static Color Alpha(Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    public static void Lerp(SpriteRenderer renderer, Color from, Color to, float duration, Action finalAction = null)
    {
        sm.StartCoroutine(_Lerp(renderer, from, to, duration, finalAction));
    }

    public static IEnumerator _Lerp(SpriteRenderer renderer, Color from, Color to, float duration, Action finalAction = null)
    {
        var passedTime = 0f;
        while (passedTime <= duration)
        {
            passedTime += Time.deltaTime;
            renderer.color = Color.Lerp(from, to, passedTime / duration);
            yield return null;
        }
        if (finalAction != null) finalAction();
    }

    public static void LerpText(Text renderer, Color from, Color to, float duration, Action finalAction = null)
    {
        sm.StartCoroutine(_LerpText(renderer, from, to, duration, finalAction));
    }

    public static IEnumerator _LerpText(Text renderer, Color from, Color to, float duration, Action finalAction = null)
    {
        var passedTime = 0f;
        while (passedTime <= duration)
        {
            passedTime += Time.deltaTime;
            renderer.color = Color.Lerp(from, to, passedTime / duration);
            yield return null;
        }
        if (finalAction != null) finalAction();
    }
}
