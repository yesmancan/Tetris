using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
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

    public static void TestData()
    {
        GetToData();
        PostToData();
    }

    public void PlayGame()
    {
        TestData();
        SceneManager.LoadScene(0);
    }

    static IEnumerator PostToData()
    {
        WWWForm form = new WWWForm();

        string address = "https://tetris-8247d.firebaseio.com/";
        using (UnityWebRequest www = UnityWebRequest.Post(address, form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
    static IEnumerator GetToData()
    {
        string address = "https://tetris-8247d.firebaseio.com/";
        using (UnityWebRequest www = UnityWebRequest.Get(address))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
                Debug.Log("Form upload complete!");
            }
        }
    }
}
