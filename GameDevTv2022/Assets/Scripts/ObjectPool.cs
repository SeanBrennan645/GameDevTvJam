using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject PlayerTilePrefab;
    [SerializeField] [Range(0, 50)] int PoolSize = 5;
    [SerializeField] TextMeshProUGUI text;

    private GameObject[] pool;
    private int currentAvailable;

    private void Awake()
    {
        PopulatePool();
        currentAvailable = PoolSize;
        text.text = "Bodies Remaining: " + currentAvailable;
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
                currentAvailable--;
                text.text = "Bodies Remaining: " + currentAvailable;
                return;
            }
        }
    }
}
