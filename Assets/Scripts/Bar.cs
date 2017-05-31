using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {

    public Color raisedColor;
    public Color loweredColor;
    SpriteRenderer sr;
    Collider2D collider;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
        sr.color = raisedColor;
	}

    public void Raise(float duration)
    {
        Util.Lerp(sr, loweredColor, raisedColor, duration, () => collider.enabled = true);
    }

    public void Lower(float duration)
    {
        Util.Lerp(sr, raisedColor, loweredColor, duration, () => collider.enabled = false);
    }
	
	
}
