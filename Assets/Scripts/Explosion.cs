using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Explosion : MonoBehaviour {

    public int parts = 5;
    public float force = 10;
    public float delay = 1;
    public float radius = 5;
    public float passThrough = 0.3f;
    public LayerMask affected;


    public void Explode()
    {
        Physics.OverlapSphere(transform.position, radius, affected).
            Where(c => Util.LineOfSight(gameObject, c.gameObject) && c.GetComponent<Absorption>() != null && c.GetComponent<Rigidbody2D>() != null).
            SelectMany(c => c.GetComponent<Absorption>().Explode(parts, passThrough)).
            Select(a => a.GetComponent<Rigidbody2D>()).ToList().
            ForEach(r => r.AddExplosionForce(force, transform.position, radius));
    }
}
