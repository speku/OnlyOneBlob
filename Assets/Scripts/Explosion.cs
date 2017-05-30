using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    public int parts = 5;
    public float force = 10;
    public float delay = 1;
    public float radius = 5;
    public float passThrough = 0.3f;
    public LayerMask affected;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Explode()
    {
        foreach (var c in Physics.OverlapSphere(transform.position, radius, affected))
        {
            var a = c.GetComponent<Absorption>();
            if (a != null) a.Explode(parts, force, passThrough).ForEach(ab => ab.rb.AddExplosionForce(force, transform.position, radius));
        }
    }
}
