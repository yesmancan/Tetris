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

    public void PlayGame()
    {
        SceneManager.LoadScene(0);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(1);
    }
    public void SetPoint()
    {
        if (point != null)
        {
            point.text = "Point : " + PlayerPrefs.GetInt("point");
        }
    }
}
