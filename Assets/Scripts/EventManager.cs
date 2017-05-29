using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {


    public event Action<Absorption> AreaChanged = delegate { };

    void Start () {
		
	}
	

	void Update () {
		
	}
}
