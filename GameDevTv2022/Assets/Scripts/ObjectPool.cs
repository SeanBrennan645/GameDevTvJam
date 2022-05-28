using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject PlayerTilePrefab;
    [SerializeField] [Range(0, 50)] int PoolSize = 5;

    private GameObject[] pool;

    private void Awake()
    {
        PopulatePool();
    }

    private void PopulatePool()
    {
        pool = new GameObject[PoolSize];

        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(PlayerTilePrefab, transform);
            pool[i].SetActive(false);
        }
    }

    public void EnableObjectInPool(Vector3 position)
    {
        for (int i = 0; i < pool.Length; i++)
        {
            if(pool[i].activeInHierarchy==false)
            {
                pool[i].transform.position = position;
                pool[i].SetActive(true);
                return;
            }
        }
    }
}
