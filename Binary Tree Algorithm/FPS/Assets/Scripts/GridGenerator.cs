using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour {
    [SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform boundaryPrefab;
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private int size;
    private Transform[][] grid;
    private bool[][] state;

    private void Start()
    {
        grid = new Transform[size+1][];
        state = new bool[size + 1][];
        for (int i = 0; i <= size; i++)
        {
            grid[i] = new Transform[size + 1];
            state[i] = new bool[size + 1];
        }
        MakeAllBlocks();
    }

    private void MakeAllBlocks()
    {
        Vector3 startPosition = new Vector3(-size / 2, -size / 2, 0f);
        MakeBoundaries();
        Vector3 pos = startPosition;
        for (int j = 0; j <= size; j++)
        {
            for (int i = 0; i <= size; i++)
            {
                Transform block = Instantiate(blockPrefab, pos, Quaternion.identity) as Transform;
                pos += new Vector3(1f, 0f, 0f);
                grid[j][i] = block;
                state[j][i] = true;
            }
            pos = startPosition + new Vector3(0f, (j+1), 0f);
        }
        state[0][0] = false;
    }

    private void MakeBoundaries()
    {
        Transform bound = Instantiate(boundaryPrefab, new Vector3(-(size / 2 + 1), 0f, 0f),
            Quaternion.identity);
        bound.localScale = new Vector3(1f, size + 2, 1f);
        bound = Instantiate(boundaryPrefab, new Vector3((size / 2 + 1), 0f, 0f),
           Quaternion.identity);
        bound.localScale = new Vector3(1f, size + 2, 1f);
        bound = Instantiate(boundaryPrefab, new Vector3(0f, (size / 2 + 1), 0f),
           Quaternion.identity);
        bound.localScale = new Vector3(size + 1, 1f, 1f);
        bound = Instantiate(boundaryPrefab, new Vector3(0f, -(size / 2 + 1), 0f),
           Quaternion.identity);
        bound.localScale = new Vector3(size + 1, 1f, 1f);
    }

    private void BinaryTree()
    {
        for (int j = 0; j < size; j++)
        {
            for (int i = 0; i < size; i++)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                    state[j+1][i] = false;
                else
                    state[j][i + 1] = false;
            }
        }
        for(int i = 0,j = size;i<=size;i++)
        {
            state[j][i] = false;
        }
        for (int i = size, j = 0; j <= size; j++)
        {
            state[j][i] = false;
        }
        RandomGenerate();
    }

    private void RandomGenerate()
    {
        for (int j = 0; j <= size; j++)
        {
            for (int i = 0; i <= size; i++)
            {
                if (state[j][i] == false)
                    grid[j][i].gameObject.SetActive(false);
            }
        }
        Transform player = Instantiate(playerPrefab,grid[0][0].transform.position,Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int j = 0; j <= size; j++)
            {
                for (int i = 0; i <= size; i++)
                {
                    state[j][i] = true;
                    grid[j][i].gameObject.SetActive(true);
                }
            }
            state[0][0] = false;
            BinaryTree();
        }
    }
}
