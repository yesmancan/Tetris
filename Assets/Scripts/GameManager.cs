using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI point;
    public TextMeshProUGUI level;
    public TextMeshProUGUI lines;
    public GameObject stopButton;
    public GameObject playButton;

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
    public static void PlayGame()
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
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
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
