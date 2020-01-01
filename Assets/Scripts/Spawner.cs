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
    public Vector2 speed = new Vector2(1f, 1f);
    void Start()
    {
        CreateNewBlock();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateNewBlock();
        }

    }

    public void CreateNewBlock()
    {
        int r = Random.Range(0, blocks.Length);
        activeObject = blocks[r];
        activeBlock = activeObject.GetComponent<Block>();
        Instantiate(activeObject, new Vector3(5f, 16, 1f), Quaternion.identity);
    }
}