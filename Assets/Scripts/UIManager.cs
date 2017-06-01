using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

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
        if (!initialized)
        {
            ShowLevels(false, true);
            OnGameStart();
            initialized = true;
        } else
        {
            title.enabled = false;
            ShowLevels(false, false);

        }
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            var b = !levels.activeSelf;
            ShowLevels(b, b);
        }
	}

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
        ShowLevels(false, false);
    }


    public void Exit()
    {
        Application.Quit();
    }


    public void OnGameStart()
    {
        Util.Delay(ShowTitle, showTitleDuration, () => ShowLevels(true, true));
    }

    public void OnGameStateChanged(string message, GameManager.GameState gs)
    {
        Util.Delay(() => ShowMessage(message, gs), showMessageDuration, () => ShowLevels(true, true));
    }

    void ShowMessage(string msg, GameManager.GameState gs)
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = true;
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
            () => { text.enabled = true; Util.LerpText(text, Util.Alpha(color, startAlpha), Util.Alpha(color, endAlpha), half); }, 
            half, 
            () => {Util.LerpText(text, Util.Alpha(color, endAlpha), Util.Alpha(color, startAlpha), half); });
        Util.Delay(null, duration, () => text.enabled = false);
    }

    public void ShowLevels(bool show, bool blur)
    {
        levels.SetActive(show);
        Camera.main.GetComponent<BlurOptimized>().enabled = blur;
    }
}
