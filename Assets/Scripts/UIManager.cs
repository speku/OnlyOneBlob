using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject levels;
    public Text title;
    public Text message;
    public float showTitleDuration;
    public float showMessageDuration;
    public Color titleColor;
    public Color messageColorWon;
    public Color messageColorLost;
    public float startAlpha;
    public float endAlpha;
    public Canvas canvas;
    static bool initialized = false;

	void Start () {
        OnGameStart();
        initialized = true;
	}
	
	void Update () {
		
	}

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
        canvas.gameObject.SetActive(false);
    }


    public void Exit()
    {
        Application.Quit();
    }


    public void OnGameStart()
    {
        canvas.gameObject.SetActive(true);
        Util.Delay(ShowTitle, showTitleDuration, () => ShowLevels(true));
    }

    public void OnGameStateChanged(string message, GameManager.GameState gs)
    {
        canvas.gameObject.SetActive(true);
        Util.Delay(() => ShowMessage(message, gs), showMessageDuration, () => ShowLevels(true));
    }

    void ShowMessage(string msg, GameManager.GameState gs)
    {
        message.text = msg;
        Show(message, gs == GameManager.GameState.Won ? messageColorWon : messageColorLost, showMessageDuration);
    }

    void ShowTitle()
    {
        Show(title, titleColor, showTitleDuration);
    }

    void Show(Text text, Color color, float duration)
    {
        var half = duration / 2;
        Util.Delay(
            () => Util.LerpText(text, Util.Alpha(color, startAlpha), Util.Alpha(color, endAlpha), half), 
            half, 
            () => Util.LerpText(text, Util.Alpha(color, endAlpha), Util.Alpha(color, startAlpha), half));
    }

    public void ShowLevels(bool show)
    {
        levels.SetActive(show);
    }
}
