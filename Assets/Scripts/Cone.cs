using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour {

    public float fadeDuration = 2;
    public float fadeDelay = 1;

    public LayerMask affected;
    public List<SpriteRenderer> landingStrip = new List<SpriteRenderer>();
    public List<GameObject> affectedObjects = new List<GameObject>();

    private bool animating = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == affected &&
            collision.gameObject.GetComponent<Rigidbody2D>() != null &&
            collision.gameObject.GetComponent<SpriteRenderer>() != null) affectedObjects.Add(collision.gameObject);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        affectedObjects.Remove(collision.gameObject);
    }

    private IEnumerator Animation()
    {
        while (animating)
        {

        }
        yield break;
    }

    private void Animate(bool start)
    {

    }
}
