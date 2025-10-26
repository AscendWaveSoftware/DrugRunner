using System.Collections.Generic;
using UnityEngine;

public class DrugObstaclePooler : MonoBehaviour
{
    [SerializeField] private DrugPrefabSet[] sets;

    private readonly Dictionary<DrugType, List<Queue<GameObject>>> pools = new();

    private void Awake()
    {
        foreach(var set in sets)
        {
            var list = new List<Queue<GameObject>>();
            for(int i = 0; i < set.prefabs.Length; i++)
            {
                var q = new Queue<GameObject>();
                for(int n = 0; n < set.sizePerPrefab; n++)
                {
                    var go = Instantiate(set.prefabs[i], transform);
                    go.SetActive(false);
                    q.Enqueue(go);
                }
                list.Add(q);
            }
            pools[set.type] = list;
        }
    }

    public GameObject Get(DrugType _type, out int prefabIndex)
    {
        var list = pools[_type];
        prefabIndex = Random.Range(0, list.Count);
        var q = list[prefabIndex];

        if(q.Count == 0)
        {
            var src = GetSourcePrefab(_type, prefabIndex);
            var extra = Instantiate(src, transform);
            extra.SetActive(false);
            return extra;
        }

        var go = q.Dequeue();
        return go;
    }

    public void Return(DrugType _type, int _prefabIndex, GameObject _go)
    {
        _go.SetActive(false);
        _go.transform.SetParent(transform);
        pools[_type][_prefabIndex].Enqueue(_go);
    }

    private GameObject GetSourcePrefab(DrugType _type, int _index)
    {
        foreach (var s in sets)
            if (s.type == _type)
                return s.prefabs[_index];
        return null;
    }
}
