using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour {


    public event Action AreaChanged;
    public event Action<GameManager.GameState> GameStateChanged;

    public void RaiseAreaChanged()
    {
        if (AreaChanged != null) AreaChanged();
    }

    public void RaiseGameStateChanged(GameManager.GameState gs)
    {
        if (GameStateChanged != null) GameStateChanged(gs);
    }

    void Start () {
		
	}
	

	void Update () {
		
	}
}
