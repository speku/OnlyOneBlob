using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Explosion : MonoBehaviour {

    public int parts = 5;
    public float force = 10;
    public float delay = 1;
    public float radius = 5;
    public float passThrough = 1;
    public float armDuration = 1;
    public LayerMask affected;


    public void Explode()
    {
        Physics2D.OverlapCircleAll(transform.position, radius, affected).
            Where(c => Util.LineOfSight(gameObject, c.gameObject) && c.GetComponent<Absorption>() != null && c.GetComponent<Rigidbody2D>() != null && c.GetComponent<PlayerMovement>() == null).
            SelectMany(c => c.GetComponent<Absorption>().Explode(parts, passThrough)).
            Select(a => a.GetComponent<Rigidbody2D>()).ToList().
            ForEach(r => r.AddExplosionForce(force, transform.position, radius));
    }

    public void Arm()
    {
        transform.SetParent(null);
        transform.localScale = Vector3.one;
        Util.Delay(null, armDuration, () => { Explode(); Destroy(gameObject); });
        GetComponent<PowerUp>().Animate(armDuration);
    }


}
