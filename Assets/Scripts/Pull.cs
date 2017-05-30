using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Pull : MonoBehaviour {

    public GameObject cone;
    public LayerMask affected;
    public float force = 10;

    List<GameObject> affectedObjects = new List<GameObject>();

    [HideInInspector]
    bool Active { get; set; }

    private void Start()
    {
        
    }

    private IEnumerator PullIn()
    {
        for (;;)
        {
            if (Active)
                affectedObjects.
                    Where(o => Util.LineOfSight(gameObject, o)).
                    Select(o => o.GetComponent<Rigidbody2D>()).ToList().
                    ForEach(r => r.ApplyRelativeForce(transform.position, force, r.GetComponent<SpriteRenderer>().bounds.size.x, Time.deltaTime));
        }
    }

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
}
