using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI point;
    public GameObject gameOverPanel;
    #region Singleton
    public static GameManager instance;
    public GameManager()
    {
        instance = this;
    }
    #endregion

    public static void PlayGame()
    {
        PlayerPrefs.SetInt("point", 0);
        SceneManager.LoadScene(1);
        LevelLoader.instance.LoadLevel(1);
    }

    void Update()
    {

    }

    public void BackToMenu()
    {
        LevelLoader.instance.LoadLevel(1);
    }
    public void SetPoint()
    {
        if (point != null)
        {
            point.text = "Point : " + PlayerPrefs.GetInt("point");
        }
    }
}
