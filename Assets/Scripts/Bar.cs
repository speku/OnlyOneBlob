using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {

    public Color raisedColor;
    public Color loweredColor;
    SpriteRenderer sr;
    Collider2D c;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
        c = GetComponent<Collider2D>();
        sr.color = raisedColor;
	}

    public void Raise(float duration)
    {
        Util.Lerp(sr, loweredColor, raisedColor, duration, () => c.enabled = true);
    }

    public void Lower(float duration)
    {
        Util.Lerp(sr, raisedColor, loweredColor, duration, () => c.enabled = false);
    }
	
	
}
