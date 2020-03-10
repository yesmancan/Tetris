using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI point;
    public TextMeshProUGUI level;
    public TextMeshProUGUI lines;
    public GameObject stopButton;
    public GameObject playButton;
    public GameObject stopMusicButton;
    public GameObject playMusicButton;

    public GameObject showDialog;

    private Block _activeBlock;

    public GameObject gameOverPanel;
    public LevelLoader _levelLoader;

    #region Singleton
    public static GameManager instance;
    public GameManager()
    {
        instance = this;
    }
    #endregion
    public void Awake()
    {
        PlayerPrefs.SetInt("point", 0);
        PlayerPrefs.SetInt("lines", 0);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.Save();

        SetPoint();

        if ((Screen.height == 2960 && Screen.width == 1440) || (Screen.height == 2340 && Screen.width == 1080))
        {
            Camera.main.fieldOfView = 70;
        }
    }
    public void Update()
    {
        _activeBlock = Spawner.instance.activeBlock;
    }
    public void LeftClick() { _activeBlock.LeftClick(); }
    public void RightClick() { _activeBlock.RightClick(); }
    public void UpClick() { _activeBlock.UpClick(); }
    public void DownClick() { _activeBlock.DownClick(); }

    public void StopAndPlay()
    {
        if (Time.timeScale == 0)
        {
            if (stopButton != null)
            {
                stopButton.SetActive(true);
            }
            if (playButton != null)
            {
                playButton.SetActive(false);
            }

            Time.timeScale = 1;
        }
        else
        {
            if (stopButton != null)
            {
                stopButton.SetActive(false);
            }
            if (playButton != null)
            {
                playButton.SetActive(true);
            }

            Time.timeScale = 0;
        }
    }

    public void MuteAndPlayMusic(bool mute)
    {
        if (mute)
        {
            stopMusicButton.SetActive(true);
            playMusicButton.SetActive(false);
            PlayerPrefs.SetInt("playMusicActive", 0);
        }
        else
        {
            stopMusicButton.SetActive(false);
            playMusicButton.SetActive(true);
            PlayerPrefs.SetInt("playMusicActive", 1);
        }
    }
    public static void PlayGame()
    {
        PlayerPrefs.SetInt("point", 0);
        PlayerPrefs.SetInt("lines", 0);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void PlayGameClick()
    {
        PlayerPrefs.SetInt("point", 0);
        PlayerPrefs.SetInt("lines", 0);
        PlayerPrefs.SetInt("level", 1);
        PlayerPrefs.Save();

        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void RefreshOpenDialog()
    {
        if (showDialog != null)
        {
            showDialog.SetActive(true);
            StopAndPlay();
        }
    }
    public void CloseOpenDialog()
    {
        if (showDialog != null)
        {
            showDialog.SetActive(false);
            StopAndPlay();
        }
    }

    public void SetPoint()
    {
        if (point != null)
            point.text = "SCORE \n" + PlayerPrefs.GetInt("point");

        if (level != null)
            level.text = "LEVEL \n" + PlayerPrefs.GetInt("level");

        if (lines != null)
            lines.text = "LINES \n" + PlayerPrefs.GetInt("lines");
    }
}
