﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float forwardSpeed = 10;
    public float rotationSpeed = 10;

    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        rb.AddForce(Input.GetAxis("Vertical") * transform.up * forwardSpeed * Time.deltaTime, ForceMode2D.Force);
        transform.Rotate(Vector3.back * Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime);
	}
}
