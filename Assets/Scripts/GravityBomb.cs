using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GravityBomb : MonoBehaviour {

    public LayerMask affected;
    public float radius = 5;
    public float force = 10;
    public float duration = 3;
    public float update = 0.1f;
    public float armDuration = 1;

    public void Explode()
    {
        var start = Time.time;
        Util.While(
            () => Time.time - start <= duration,
            () => {
            Physics2D.OverlapCircleAll(transform.position, radius, affected).Where(c => Util.LineOfSight(gameObject, c.gameObject) && c.GetComponent<Rigidbody2D>() != null).ToList().ForEach(c =>
            {
                var dir = transform.position - c.transform.position;
                var intensity = 1 - (dir.magnitude / radius);
                c.GetComponent<Rigidbody2D>().AddForce(dir.normalized * intensity * force * Time.deltaTime);
            });}, 
        update,
        () => Destroy(gameObject));
    }

    public void Arm()
    {
        Util.Delay(null, armDuration, Explode);
        GetComponent<PowerUp>().Animate(duration + 1);
    }
}
