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
    public GameObject nextObjectPanel { get; set; }

    public Vector2 speed = new Vector2(1f, 1f);
    public int nextBlockIndex { get; set; }
    void Start()
    {
        CreateNewBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CreateNewBlock();
    }

    public void CreateNewBlock(int _nextBlockIndex = 0)
    {
        int r = _nextBlockIndex;
        if (!r.Equals(0))
            r = Random.Range(0, blocks.Length);

        activeObject = blocks[r];
        activeBlock = activeObject.GetComponent<Block>();

        Instantiate(activeObject, new Vector3(5f, 16, 1f), Quaternion.identity);
        nextBlockIndex = Random.Range(0, blocks.Length);

        Instantiate(blocks[nextBlockIndex], new Vector3(5f, 16, 1f), Quaternion.identity);

    }
}