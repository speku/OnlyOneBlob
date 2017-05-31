using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lock : MonoBehaviour {

    public Color backgroundClosedColor;
    public Color backgroundOpenColor;
    public Color symbolClosedColor;
    public Color symbolOpenColor;
    public Color stripClosedColor;
    public Color stripOpenColor;
    public SpriteRenderer background;
    public SpriteRenderer symbol;
    public float unlockAnimationDuration;
    public Gate gate;
    public List<SpriteRenderer> activationStrip = new List<SpriteRenderer>();
    public float unlockArea = 1;
    public float stripAnimationDuration;
    public float stripAnimationDelay;
    public float indicatorUpdatePeriod;
    List<Absorption> blobs = new List<Absorption>();
    public List<Absorption> blobsToUnlock = new List<Absorption>();
    bool unlocked = false;

	void Start () {
        background.color = backgroundClosedColor;
        symbol.color = symbolClosedColor;
        StartCoroutine(IndicateProgress());
	}
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var a = collision.GetComponent<Absorption>();
        if (a == null) return;
        blobs.Add(a);
        if (a.Area() >= UnlockArea()) Unlock();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var a = collision.GetComponent<Absorption>();
        if (a == null) return;
        blobs.Remove(a);
    }
    

    float UnlockArea()
    {
        return blobsToUnlock.Count > 0 ? blobsToUnlock.Select(b => b.Area()).Sum() : unlockArea;
    }

    IEnumerator IndicateProgress()
    {
        while(!unlocked)
        {
            var max = blobs.Select(b => b.Area()).DefaultIfEmpty().Max();
            var p = max / UnlockArea();
            background.color = Color.Lerp(backgroundClosedColor, backgroundOpenColor, p);
            symbol.color = Color.Lerp(symbolClosedColor, symbolOpenColor, p);
            if (max >= UnlockArea()) Unlock();
            yield return new WaitForSeconds(indicatorUpdatePeriod);
        }

    }



    void Unlock()
    {
        if (unlocked) return;
        unlocked = true;
        StartCoroutine(_Unlock());
    }

    IEnumerator _Unlock()
    {
        Util.Lerp(symbol, symbolClosedColor, symbolOpenColor, unlockAnimationDuration);
        Util.Lerp(background, backgroundClosedColor, backgroundOpenColor, unlockAnimationDuration);
        yield return new WaitForSeconds(unlockAnimationDuration);
        foreach (var sr in activationStrip)
        {
            AnimateStrip(sr);
            yield return new WaitForSeconds(stripAnimationDelay);
        }
        gate.Open();
    }

    public void AnimateStrip(SpriteRenderer sr)
    {
        Util.Lerp(sr, stripClosedColor, stripOpenColor, stripAnimationDuration);
    }

}
