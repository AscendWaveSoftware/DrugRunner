using UnityEngine;
using UnityEngine.SceneManagement;

public class DrugObstacle : MonoBehaviour
{
    [HideInInspector] public DrugObstaclePooler pooler;
    [HideInInspector] public DrugType drugType;
    [HideInInspector] public int prefabIndex;
    [SerializeField] private float maxLifetime = 10f;
    private float lifetime;
    private bool isDespawning;

    private float lifeTime;

    private void OnEnable()
    {
        lifeTime = 0f;
        isDespawning = false;
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
        // Despawn();
        if (collision.collider.CompareTag("Player") && DrugsManager.playerCanDie == true)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
            AppReloader.Restart();
        }
    }

    public void Despawn()
    {
        if (isDespawning) return;
        isDespawning = true;

        
        if (pooler == null || prefabIndex < 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if (TryGetComponent<Collider>(out var col)) col.enabled = false;
        pooler.Return(drugType, prefabIndex, gameObject);
    }
}
