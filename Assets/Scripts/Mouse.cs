using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour {

    public float distance = 3;
    PlayerMovement player;
    SpriteRenderer sr;
    public LayerMask collideWith;

	void Start () {
        sr = GetComponent<SpriteRenderer>();
        player = FindObjectOfType<PlayerMovement>();

    }

	void Update () {
        UpdateCursorPosition();
    }

    private void FixedUpdate()
    {
       
    }

    public void Show(bool show = true)
    {
        sr.enabled = show;
    }


    void UpdateCursorPosition()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }
        var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        var posLimit = player.transform.position + pos.normalized * distance;
        var posFinal = pos.magnitude <= posLimit.magnitude ? pos : posLimit;
        //var c = Physics2D.Linecast(transform.position, posFinal, collideWith);
        //if (c.collider != null) posFinal = c.transform.position;
        transform.position = posFinal;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position - player.transform.position);
    }




}
