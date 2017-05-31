using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {


    public event Action AreaChanged;

    public void RaiseAreaChanged()
    {
        if (AreaChanged != null) AreaChanged();
    }

    void Start () {
		
	}
	

	void Update () {
		
	}
}
