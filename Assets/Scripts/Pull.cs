using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pull : MonoBehaviour {

    public Cone cone;
    public PlayerMovement player;
    public float force = 10;

    [HideInInspector]
    bool Active { get; set; }

    private void Start()
    {
        cone = FindObjectOfType<Cone>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private IEnumerator PullIn()
    {
        for (;;)
        {
            if (Active)
                cone.affectedObjects.
                    Where(o => Util.LineOfSight(gameObject, o)).
                    Select(o => o.GetComponent<Rigidbody2D>()).ToList().
                    ForEach(r => r.ApplyRelativeForce(transform.position, force, r.GetComponent<SpriteRenderer>().bounds.size.x, Time.deltaTime));
        }
    }
}
