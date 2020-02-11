using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    #region Singleton
    public static Spawner instance;

    public Spawner()
    {
        instance = this;
    }
    #endregion

    public GameObject[] blocks;
    public Block activeBlock;
    public GameObject activeObject;
    public GameObject nextObjectPanel;

    public Vector2 speed = new Vector2(1f, 1f);

    System.Random random = new System.Random();
    void Start()
    {
        CreateNewBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateNewBlock(false);
    }

    public void CreateNewBlock(bool useNext = true)
    {
        int r = -1;
        if (useNext)
            r = PlayerPrefs.GetInt("nextBlockIndex");

        if (r.Equals(-1))
            r = random.Next(blocks.ToList().Count());

        activeObject = blocks[r];
        activeBlock = activeObject.GetComponent<Block>();

        Instantiate(activeObject, new Vector3(5f, 20, 1f), Quaternion.identity);
        PlayerPrefs.SetInt("nextBlockIndex", random.Next(blocks.ToList().Count()));

        GameObject nextObject = Resources.Load("Models/Prefabs/BlockEmpty/" + blocks[PlayerPrefs.GetInt("nextBlockIndex")].name, typeof(GameObject)) as GameObject; ;
        var _nextOb = Instantiate(nextObject, nextObjectPanel.transform.position, Quaternion.identity);
        _nextOb.transform.parent = nextObjectPanel.transform;

        if (nextObjectPanel.gameObject.transform.childCount > 1)
            Destroy(nextObjectPanel.gameObject.transform.GetChild(0).gameObject);
    }
}