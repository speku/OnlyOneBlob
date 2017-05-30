using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rigidbody2DExt {

    public static void AddExplosionForce(this Rigidbody2D body, float force, Vector3 origin, float radius)
    {
        var dir = body.transform.position - origin;
        body.AddForce(dir.normalized * (1 - (dir.magnitude / radius)) * force);
    }
}
