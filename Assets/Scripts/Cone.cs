using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cone : MonoBehaviour {

    public float fadeSpeed = 1;
    public float fadeDelay = 1;

    public LayerMask affected;
    public List<SpriteRenderer> landingStrip = new List<SpriteRenderer>();
    public List<GameObject> affectedObjects = new List<GameObject>();


    private bool animating = false;

    private void Start()
    {
        Show(false);
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.layer == affected &&
            c.gameObject.GetComponent<Rigidbody2D>() != null &&
            c.gameObject.GetComponent<SpriteRenderer>() != null) affectedObjects.Add(c.gameObject);
    }


    private void OnTriggerExit2D(Collider2D c)
    {
        affectedObjects.Remove(c.gameObject);
    }

    private IEnumerator Animation()
    {
        while (animating)
        {
            foreach (var sr in landingStrip)
            {
                Util.Alpha(sr, 1);
                Util.Fade(sr, fadeSpeed);
                yield return new WaitForSeconds(fadeDelay);
            }
        }
        yield break;
    }

    public void Show(bool show = true)
    {
        gameObject.SetActive(show);
        //GetComponent<Collider2D>().enabled = show;
        animating = show;
        if (show) StartCoroutine(Animation());
    }
}
