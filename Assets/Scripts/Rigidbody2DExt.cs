using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rigidbody2DExt {

    public static void AddExplosionForce(this Rigidbody2D body, float force, Vector3 origin, float radius)
    {
        var dir = body.transform.position - origin;
        body.AddForce(dir.normalized * (1 - (dir.magnitude / radius)) * force);
    }

    public static void ApplyRelativeForce(this Rigidbody2D from, Vector3 to, float force, float maxDistance, float deltaTime = 1, bool inverted = false)
    {
        var dir = to - from.transform.position;
        var p = dir.magnitude / maxDistance;
        var intensity = inverted ? p : 1 - p;
        from.AddForce(dir.normalized * p * force * deltaTime);
    }
}
