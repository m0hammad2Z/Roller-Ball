using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    //public int score;
    public bool isPlay;
    //public bool isVibrate = true;
    public LSystem lSystem;

    [Header("Ads")]
    [Space]
    [Header("Continue Reward Ad")]
    public BallController ballController;
    public GameObject counter;
    public ShopSystem shopSystem;
    public static bool isCounting;

    [Header("UI")]
    [Space]
    [Header("Start Panel")]
    public GameObject StartPanel;
    public TextMeshProUGUI lvlText;
    [Header("Game Panel")]
    public GameObject gamePanel;

    [Header("Level Complete Panel")]
    public GameObject LCPanel;
    public TextMeshProUGUI text;


    [Header("Pause Game Panel")]
    public GameObject pauseGamePanel;
    public Image voiceButtonPGP;
    public Sprite muteSpriteGP, unMuteSpriteGP;

    [Header("Game Over Panel")]
    public GameObject gameOverPanel;
    public Image voiceButtonOGP;
    public Sprite muteSpriteOGP, unMuteSpriteOGP;


    [Header("Shop Panel")]
    public GameObject shopPanel;
    public GameObject ballsPanel, cylinderPanel;
    public bool activeBallsPanel;

    [Header("Voice UI")]
    public Sprite muteSprite;
    public Sprite unMuteSprite;
    public Button startVoiceButton, pauseVoiveButton, loseVoiceButton;

    //[Header("Vibration UI")]
    //public Sprite vibrateSprite;
    //public Sprite unVibrateSprite;
    //public Button startVibrateButton, pauseVibrateButton, loseVibrateButton;

    public GameObject wholeGameFnish;

    void Start()
    {
        activeBallsPanel = true;
        OnOpenScene();
        //isVibrate = PlayerPrefs.GetInt("vibrate") == 1 ? true : false;

        //Voice
        if (AudioManager.muteGameMusic == true)
        {
            startVoiceButton.image.sprite = muteSprite;
            pauseVoiveButton.image.sprite = muteSprite;
            loseVoiceButton.image.sprite = muteSprite;

        }
        else
        {
            startVoiceButton.image.sprite = unMuteSprite;
            pauseVoiveButton.image.sprite = unMuteSprite;
            loseVoiceButton.image.sprite = unMuteSprite;
        }


        ////Vibration
        //if (isVibrate == false)
        //{
        //    startVibrateButton.image.sprite = unVibrateSprite;
        //    pauseVibrateButton.image.sprite = unVibrateSprite;
        //    loseVibrateButton.image.sprite = unVibrateSprite;
        //}
        //else
        //{

        //    startVibrateButton.image.sprite = vibrateSprite;
        //    pauseVibrateButton.image.sprite = vibrateSprite;
        //    loseVibrateButton.image.sprite = vibrateSprite;
        //}
    }


    public void OnOpenScene()
    {
        Time.timeScale = 0;
        isPlay = false;
        //if(isPlay)
        //InvokeRepeating("ScoreUp", 0.2f, 0.1f);


        gamePanel.SetActive(false);
        StartPanel.SetActive(true);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        shopPanel.SetActive(false);
    }

    public void OnClickStart()
    {
        Time.timeScale = 1;
        StartPanel.SetActive(false);
        gamePanel.SetActive(true);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        shopPanel.SetActive(false);
        isPlay = true;
    }

    public void OnGameOver()
    {
        Time.timeScale = 0;
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        StartPanel.SetActive(false);

        isPlay = false;
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        pauseGamePanel.SetActive(true);
        isPlay = false;
    }
    public void OnContinue()
    {
        StartCoroutine(Counter());
    }

    IEnumerator Counter()
    {
        isCounting = true;
        Obstacle.moveSpeed = 0;
        counter.SetActive(true);
        OnClickStart();
        ballController.childBall.gameObject.SetActive(true);

        GameObject clone = GameObject.FindGameObjectWithTag("clone");
        
        
            Destroy(clone);
        

        counter.GetComponent<TextMeshProUGUI>().SetText("3");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("2");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("1");
        yield return new WaitForSeconds(1);

        counter.SetActive(false);
        Obstacle.moveSpeed = 22;
        isCounting = false;
    }

    public void OnResume()
    {
        StartCoroutine(ResumeCounter());
    }

    IEnumerator ResumeCounter()
    {
        isCounting = true;
        Obstacle.moveSpeed = 0;
        counter.SetActive(true);
        OnClickStart();
        ballController.childBall.gameObject.SetActive(true);
        

        counter.GetComponent<TextMeshProUGUI>().SetText("3");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("2");
        yield return new WaitForSeconds(1);
        counter.GetComponent<TextMeshProUGUI>().SetText("1");
        yield return new WaitForSeconds(1);

        counter.SetActive(false);
        Obstacle.moveSpeed = 22;
        isCounting = false;
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Game");
        OnOpenScene();
    }

    public void OnLevelComplete()
    {
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        LCPanel.SetActive(true);
        pauseGamePanel.SetActive(false);
        StartPanel.SetActive(false);
        lvlText.gameObject.SetActive(false);

        text.SetText("Level " + lSystem.levels[lSystem.currentLevel].levelID + " Completed");

        Time.timeScale = 0;
        isPlay = false;
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        StartPanel.SetActive(false);
        gamePanel.SetActive(false);
        LCPanel.SetActive(false);
        pauseGamePanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    public void SwitchShopPanels()
    {
        if (activeBallsPanel)
        {
            activeBallsPanel = false;
        }
        else if (!activeBallsPanel)
        {
            activeBallsPanel = true;
        }
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        OnOpenScene();
    }

    public void NextLevel()
    {
        if (lSystem.currentLevel >= lSystem.levels.Count)
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Game");

        }
        else
        {
            lSystem.currentLevel++;
            lSystem.SaveLevelNumber();
            SceneManager.LoadScene("Game");
            GetComponent<InterstitialAd>().ShowAd();
        }
    }


    public void ControlVoice()
    {
        if (AudioManager.muteGameMusic == true)
        {
            startVoiceButton.image.sprite = unMuteSprite;
            pauseVoiveButton.image.sprite = unMuteSprite;
            loseVoiceButton.image.sprite = unMuteSprite;

            AudioManager.UnMute();
        }
        else
        {

            startVoiceButton.image.sprite = muteSprite;
            pauseVoiveButton.image.sprite = muteSprite;
            loseVoiceButton.image.sprite = muteSprite;

            AudioManager.Mute();
        }
    }

    //public void ControlVibration()
    //{
    //    if (isVibrate == true)
    //    {
    //        startVibrateButton.image.sprite = unVibrateSprite;
    //        pauseVibrateButton.image.sprite = unVibrateSprite;
    //        loseVibrateButton.image.sprite = unVibrateSprite;

    //        isVibrate = false;
    //    }
    //    else
    //    {

    //        startVibrateButton.image.sprite = vibrateSprite;
    //        pauseVibrateButton.image.sprite = vibrateSprite;
    //        loseVibrateButton.image.sprite = vibrateSprite;

    //        isVibrate = true;
    //    }
    //    PlayerPrefs.SetInt("vibrate", isVibrate == false ? 0 : 1);
    //}

    public void CloseGame()
    {
        Application.Quit();
    }

    private void Update()
    {
        if(lSystem.currentLevel >= lSystem.levels.Count)
        {
            wholeGameFnish.SetActive(true);
        }
        else
        {
            lvlText.SetText("Level " + lSystem.levels[lSystem.currentLevel].levelID);
        }

        if(activeBallsPanel == true)
        {
            cylinderPanel.SetActive(false);
            ballsPanel.SetActive(true);
        }
        else
        {
            cylinderPanel.SetActive(true);
            ballsPanel.SetActive(false);
        }
    }

}
