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
    public float unlockArea;
    public float stripAnimationDuration;
    public float stripAnimationDelay;
    public float indicatorUpdatePeriod;
    List<Absorption> blobs = new List<Absorption>();
    public List<Absorption> blobsToUnlock = new List<Absorption>();

	void Start () {
        background.color = backgroundClosedColor;
        symbol.color = symbolClosedColor;
        //StartCoroutine(IndicateProgress());
	}
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var a = collision.GetComponent<Absorption>();
        if (a == null) return;
        blobs.Add(a);
        if (a.Area() >= (blobsToUnlock.Count > 0 ? blobsToUnlock.Select(b => b.Area()).Sum() : unlockArea)) Unlock();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var a = collision.GetComponent<Absorption>();
        if (a == null) return;
        blobs.Remove(a);
    }

    IEnumerator IndicateProgress()
    {
        for (;;)
        {
            var max = blobs.Select(b => b.Area()).DefaultIfEmpty(0).Max();
            var p = max / unlockArea;
            background.color = Color.Lerp(backgroundClosedColor, backgroundOpenColor, p);
            symbol.color = Color.Lerp(symbolClosedColor, symbolOpenColor, p);
        }

    }



    void Unlock()
    {
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
