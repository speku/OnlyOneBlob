using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absorption : MonoBehaviour {

    public float tinyScale = 0.01f;

    Rigidbody2D rb;
    CircleCollider2D cc;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CircleCollider2D>();
	}
	
	void Update () {
		
	}

    public void Scale(float newArea)
    {
        transform.localScale = transform.localScale * (newArea / Area());
    }

    public void DestroyTiny()
    {
        if (transform.localScale.x < tinyScale) Destroy(gameObject);
    }

    public void Absorb(Absorption other, float percentage)
    {
        if (!(Area() > other.Area())) return;
        
        var otherNewArea = other.Area() * (1 - percentage);
        var myNewArea = Area() + other.Area() * percentage;

        Scale(myNewArea);
        other.Scale(otherNewArea);
        other.DestroyTiny();
    }

    public float Area()
    {
        
        return Mathf.Pow(cc.radius, 2) * Mathf.PI;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        var absorption = c.gameObject.GetComponent<Absorption>();
        if (absorption == null) return; 
    }
}
