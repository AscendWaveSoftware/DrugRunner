using UnityEngine;

public class DrugObstacle : MonoBehaviour
{
    [HideInInspector] public DrugObstaclePooler pooler;
    [HideInInspector] public DrugType drugType;
    [HideInInspector] public int prefabIndex;
    [SerializeField] private float maxLifetime = 10f;

    private float lifeTime;

    private void OnEnable()
    {
        lifeTime = 0f;
    }

    private void Update()
    {
        lifeTime += Time.deltaTime;
        if (lifeTime >= maxLifetime)
            Despawn();
    }

    private void OnBecameInvisible()
    {
        Despawn();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Despawn();
    }

    public void Despawn()
    {
        if (pooler != null)
            pooler.Return(drugType, prefabIndex, gameObject);
    }
}
