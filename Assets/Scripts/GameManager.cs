using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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
}
