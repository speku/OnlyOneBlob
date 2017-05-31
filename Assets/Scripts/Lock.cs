using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float activationArea;
    public float stripAnimationDuration;
    public float stripAnimationDelay;

	void Start () {
		
	}
	

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var a = collision.GetComponent<Absorption>();
        if (a == null) return;
        if (a.Area() >= activationArea) Unlock();
    }

    void Unlock()
    {
        StartCoroutine(_Unlock());
    }

    IEnumerator _Unlock()
    {
        foreach (var sr in activationStrip)
        {
            AnimateStrip(sr);
            yield return new WaitForSeconds(stripAnimationDelay);
        }
        Util.Lerp(symbol, symbolClosedColor, symbolOpenColor, unlockAnimationDuration);
        Util.Lerp(background, backgroundClosedColor, backgroundOpenColor, unlockAnimationDuration, () => gate.Open());
    }

    public void AnimateStrip(SpriteRenderer sr)
    {
        Util.Lerp(sr, stripClosedColor, stripOpenColor, stripAnimationDuration);
    }

}
