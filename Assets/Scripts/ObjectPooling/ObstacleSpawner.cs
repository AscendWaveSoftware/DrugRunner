using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] float spawnIntervall = 0.1f;
    [SerializeField] ObjectPooling Pool;
    public Collider Collider;

    private float timeBetweenSpawningObjects = 0;

    [SerializeField] Transform playerPos;

    private void Update()
    {
        timeBetweenSpawningObjects += Time.deltaTime;

        if (timeBetweenSpawningObjects >= spawnIntervall)
        {
            SpawnObjectInsideSpawnBounds();
            timeBetweenSpawningObjects = 0;
        }
    }
    private void SpawnObjectInsideSpawnBounds()
    {
        if (Pool.ObjectPool.Count > 0)
        {
            var temp = Pool.ObjectPool.Pop();
            //temp.transform.position = GetRandomPosInsideBox(Collider.bounds);
            temp.transform.position = transform.position;
            temp.SetActive(true);
        }
    }

    //public void SpawnObjectFromObjectPool(Vector3 _playerPos)
    //{
    //    var temp = Pool.ObjectPool.Pop();
    //    temp.SetActive(true);
    //    temp.transform.position = _playerPos;
    //}

    private Vector3 GetRandomPosInsideBox(Bounds _collider)
    {
        return new Vector3(
            Random.Range(_collider.min.x, _collider.max.x),
            Random.Range(_collider.min.y, _collider.max.y),
            Random.Range(_collider.min.z, _collider.max.z)
            );
    }
}
