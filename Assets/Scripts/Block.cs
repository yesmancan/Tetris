using UnityEngine;

public class Block : MonoBehaviour
{
    public Vector3 rotationPoint;
    private float previosTime;
    public float fallTime = 0.8f;

    public static int width = 10;
    public static int height = 30;

    private static Transform[,] grid = new Transform[20, height];

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0);

        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.localPosition += new Vector3(-1, 0, 0);
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
        }

        if (Time.time - previosTime > (Input.GetKeyDown(KeyCode.DownArrow) ? fallTime / 10 : fallTime))
        {
            transform.position += new Vector3(0, -1, 0);
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0);
                AddToGrid();
                CheckForLines();
                this.enabled = false;
                Spawner.instance.CreateNewBlock(true);
            }
            previosTime = Time.time;
        }
    }
    void CheckForLines()
    {
        int multiple = 0;

        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                multiple++;
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
                grid[roundedX, roundedY] = children;
            }
            catch (System.Exception ex)
            {
                previosTime = 0;
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