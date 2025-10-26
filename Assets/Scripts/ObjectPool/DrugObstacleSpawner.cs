using System.Collections;
using UnityEngine;

public class DrugObstacleSpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private DrugObstaclePooler pooler;
    [SerializeField] private DrugsManager manger;

    [Header("Spawn Area (lokal relativ zu Transform)")]
    [SerializeField] private Vector2 areaSize = new Vector2(20f, 1f); // B * H
    [SerializeField] private float spawnEveryMin = 0.35f;
    [SerializeField] private float spawnEveryMax = 0.8f;

    [Header("Fall-Physik")]
    [SerializeField] private float minFallSpeed = 6f;
    [SerializeField] private float maxFallSpeed = 14f;
    [SerializeField] private bool useGravity = false;

    private Coroutine loop;
    private DrugType? lastObserved;

    private void Update()
    {
        var current = manger.CurrentActive;
        if(current != lastObserved)
        {
            lastObserved = current;
            if (loop != null)
                StopCoroutine(loop);
            if (current.HasValue)
                loop = StartCoroutine(SpawnLoop(current.Value));
        }
    }

    private IEnumerator SpawnLoop(DrugType _type)
    {
        while (manger.CurrentActive == _type)
        {
            SpawnOne(_type);
            yield return new WaitForSeconds(Random.Range(spawnEveryMin, spawnEveryMax));
        }
    }

    private void SpawnOne(DrugType _type)
    {
        var go = pooler.Get(_type, out var prefabsIndex);

        Vector3 localPos = new Vector3(
            Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
            Random.Range(0f, areaSize.y),
            0f
        );

        var worldPos = transform.TransformPoint(localPos);

        go.transform.SetParent(null);
        go.transform.position = worldPos;
        go.transform.rotation = Quaternion.identity;

        var po = go.GetComponent<DrugObstacle>();
        if (po == null)
            po = go.AddComponent<DrugObstacle>();
        po.pooler = pooler;
        po.drugType = _type;
        po.prefabIndex = prefabsIndex;

        var rb = go.GetComponent<Rigidbody>();
        if (!rb) rb = go.AddComponent<Rigidbody>();
        rb.useGravity = useGravity;
        if (!useGravity)
            rb.linearVelocity = Vector3.down * Random.Range(minFallSpeed, maxFallSpeed);

        go.SetActive(true);
    }
}
