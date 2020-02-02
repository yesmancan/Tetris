using System.Collections;
using System.Collections.Generic;
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
    public int nextBlockIndex { get; set; }
    void Start()
    {
        GameManager.TestData();

        CreateNewBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateNewBlock(false);
    }

    public void CreateNewBlock(bool useNext = true)
    {
        int r = 0;
        if (useNext)
            r = nextBlockIndex;

        if (r.Equals(0))
            r = Random.Range(0, blocks.Length);

        activeObject = blocks[r];
        activeBlock = activeObject.GetComponent<Block>();

        Instantiate(activeObject, new Vector3(5f, 16, 1f), Quaternion.identity);
        nextBlockIndex = Random.Range(0, blocks.Length);

        GameObject nextObject = Resources.Load("Models/Prefabs/BlockEmpty/" + blocks[nextBlockIndex].name, typeof(GameObject)) as GameObject; ;
        var _nextOb = Instantiate(nextObject, nextObjectPanel.transform.position, Quaternion.identity);
        _nextOb.transform.parent = nextObjectPanel.transform;

        if (nextObjectPanel.gameObject.transform.childCount > 1)
            Destroy(nextObjectPanel.gameObject.transform.GetChild(0).gameObject);
    }
}