using UnityEngine;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour
{
    [SerializeField] int objectAmountPerPrefab = 50;
    [SerializeField] List<GameObject> itemPrefabs;
    public Stack<GameObject> ObjectPool = new();
    public List<GameObject> objectPool;

    void Awake()
    {
        StartObjectPooling(objectAmountPerPrefab);
    }

    public void StartObjectPooling(int _amountPerPrefab)
    {
        for (int i = 0; i < _amountPerPrefab; i++)
        {
            foreach (var item in itemPrefabs)
            {
                var temp = Instantiate(item.gameObject);
                temp.transform.SetParent(transform);
                temp.SetActive(false);
                ObjectPool.Push(item);
            }
        }
    }
}
