using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    public EventManager em;
    public UIManager ui;

    public List<string> wonMessages = new List<string>();
    public List<string> lostMessages = new List<string>();

	void Start () {
        em.GameStateChanged += OnGameStateChanged;
	}


    void OnGameStateChanged(GameState gs)
    {
        switch (gs)
        {
            case GameState.Won:
                ui.OnGameStateChanged(wonMessages[Random.Range(0, wonMessages.Count - 1)], gs);
                break;
            case GameState.Lost:
                ui.OnGameStateChanged(lostMessages[Random.Range(0, lostMessages.Count - 1)], gs);
                break;
        }
    }
	

	void Update () {
		
	}

    private void OnDestroy()
    {
        em.GameStateChanged -= OnGameStateChanged;
    }



    public enum GameState
    {
        Won,
        Lost
    }
}
