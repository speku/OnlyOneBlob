using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {


    public event Action<Absorption> AreaChanged = delegate { };

    public void RaiseAreaChanged(Absorption a)
    {
        AreaChanged(a);
    }

    void Start () {
		
	}
	

	void Update () {
		
	}
}
