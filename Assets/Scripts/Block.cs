using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previosTime;
    public float fallTime = 1f;
    public float fallCrosser = 1f;
    public static int width = 10;
    public static int height = 30;
    private static int cleanRowCount = 0;
    private static bool gameRunningStatus = true;
    private static readonly Transform[,] grid = new Transform[20, height];

    private static bool leftActive = false;
    private static bool rightActive = false;
    private static bool downActive = false;
    private static bool fastDownActive = false;
    private static bool upActive = false;

    void Update()
    {
        if (PlayerPrefs.GetInt("level", 1) == 1)
            fallTime = 1f;
        else if (PlayerPrefs.GetInt("level", 1) == 2)
            fallTime = .8f;
        else if (PlayerPrefs.GetInt("level", 1) == 3)
            fallTime = .6f;
        else if (PlayerPrefs.GetInt("level", 1) == 4)
            fallTime = .5f;
        else if (PlayerPrefs.GetInt("level", 1) == 5)
            fallTime = .4f;
        else if (PlayerPrefs.GetInt("level", 1) == 6)
            fallTime = .3f;
        else if (PlayerPrefs.GetInt("level", 1) == 7)
            fallTime = .2f;
        else
            fallTime = .1f;


        if (!gameRunningStatus && Time.timeScale == 0)
            return;

        if ((Input.GetKeyDown(KeyCode.LeftArrow) || leftActive) && Time.timeScale != 0)
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);

            leftActive = false;
        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || rightActive) && Time.timeScale != 0)
        {
            transform.localPosition += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);

            rightActive = false;
        }
        else if ((Input.GetKeyDown(KeyCode.UpArrow) || upActive) && Time.timeScale != 0)
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);

            upActive = false;
        }

        float _fallTime = fallTime;
        if ((Input.GetKeyDown(KeyCode.DownArrow) || downActive) && Time.timeScale != 0)
            _fallTime = fallTime / 10;

        if ((Input.GetKeyDown(KeyCode.S) || fastDownActive) && Time.timeScale != 0)
            _fallTime = fallTime / 100;

        if (Time.time - previosTime > _fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();
                this.enabled = false;

                if (!gameRunningStatus && Time.timeScale == 0)
                    return;

                fastDownActive=false;
                Spawner.instance.CreateNewBlock(true);
            }
            previosTime = Time.time;
            downActive = false;
        }
    }

    public void LeftClick()
    {
        leftActive = true;
    }
    public void RightClick()
    {
        rightActive = true;
    }
    public void UpClick()
    {
        upActive = true;
    }
    public void DownClick()
    {
        downActive = true;
    }
    public void FastDownClick()
    {
        fastDownActive = true;
    }

    void CheckForLines()
    {
        int multiple = 0;

        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                multiple++;
                cleanRowCount++;
                PlayerPrefs.SetInt("lines", cleanRowCount);
                int _level = ((cleanRowCount / 10) == 0 ? 1 : cleanRowCount / 10);
                if (cleanRowCount > 10)
                {
                    int _level_ = _level + 1;
                    PlayerPrefs.SetInt("level", _level_);
                }

                DeleteLine(i);
                RowDown(i);
            }
        }

        if (multiple == 4)
            PlayerPrefs.SetInt("point", PlayerPrefs.GetInt("point") + (1200 * PlayerPrefs.GetInt("level", 1)));
        else if (multiple == 3)
            PlayerPrefs.SetInt("point", PlayerPrefs.GetInt("point") + (300 * PlayerPrefs.GetInt("level", 1)));
        else if (multiple == 2)
            PlayerPrefs.SetInt("point", PlayerPrefs.GetInt("point") + (100 * PlayerPrefs.GetInt("level", 1)));
        else if (multiple == 1)
            PlayerPrefs.SetInt("point", PlayerPrefs.GetInt("point") + (40 * PlayerPrefs.GetInt("level", 1)));

        if (multiple > 0)
            GameManager.instance.SetPoint();

    }
    bool HasLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            if (grid[j, i] == null)
                return false;
        }
        return true;
    }
    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j, i] = null;
        }
    }
    void RowDown(int i)
    {
        for (int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }
    void AddToGrid()
    {
        foreach (Transform children in transform)
        {
            try
            {
                int roundedX = Mathf.RoundToInt(children.transform.position.x);
                int roundedY = Mathf.RoundToInt(children.transform.position.y);
                Transform _grid = grid[roundedX, roundedY];
                if (_grid == null)
                {
                    grid[roundedX, roundedY] = children;
                }
                else
                {
                    // AdsRequests.admob.ShowInterstitial();

                    Time.timeScale = 0;
                    gameRunningStatus = false;

                    GameManager.instance.gameOverPanel.SetActive(true);
                    Debug.Log("GameOver");
                    break;
                }
            }
            catch (System.Exception ex)
            {
                Time.timeScale = 0;
                GameManager.instance.gameOverPanel.SetActive(true);
                Debug.LogError(ex.Message);
            }
        }
    }
    bool ValidMove()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
                return false;

            if (grid[roundedX, roundedY] != null)
                return false;
        }

        return true;
    }
}