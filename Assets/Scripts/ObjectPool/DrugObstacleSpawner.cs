using System;
using System.Collections;
using UnityEngine;

public class DrugObstacleSpawner : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private DrugObstaclePooler pooler;

    [Header("Spawn Area")]
    [SerializeField] private Vector2 areaSize = new Vector2(20f, 1f);
    [SerializeField] private float spawnEveryMin = 0.35f;
    [SerializeField] private float spawnEveryMax = 0.8f;

    [Header("Fall Physics")]
    [SerializeField] private float minFallSpeed = 6f;
    [SerializeField] private float maxFallSpeed = 14f;
    [SerializeField] private bool useGravity = false;

    [Header("Default Spawning")]
    [SerializeField] private DrugType defaultType = DrugType.Default; 

    private Coroutine currentSpawnLoop;
    private DrugType? currentDrug;


    private void Start()
    {
        StartDefault();
    }

    private void OnEnable()
    {
        // Events abonnieren
        CokeDrug.OnCokeEnabled += OnCokeEnabled;
        CokeDrug.OnCokeDisabled += OnCokeDisabled;

        HazeDrug.OnHazeEnabled += OnHazeEnabled;
        HazeDrug.OnHazeDisabled += OnHazeDisabled;

        LSDDrug.OnLSDEnabled += OnLSDEenabled;
        LSDDrug.OnLSDDisabled += OnLSDDisabled;

        HeroinDrug.OnHeroinEnabled += OnHeroinEnabled;
        HeroinDrug.OnHeroinDisabled += OnHeroinDisabled;

        StartDefault(); 
    }

    private void OnDisable()
    {
        // Events abbestellen
        CokeDrug.OnCokeEnabled -= OnCokeEnabled;
        CokeDrug.OnCokeDisabled -= OnCokeDisabled;

        HazeDrug.OnHazeEnabled -= OnHazeEnabled;
        HazeDrug.OnHazeDisabled -= OnHazeDisabled;

        LSDDrug.OnLSDEnabled -= OnLSDEenabled;
        LSDDrug.OnLSDDisabled -= OnLSDDisabled;

        HeroinDrug.OnHeroinEnabled -= OnHeroinEnabled;
        HeroinDrug.OnHeroinDisabled -= OnHeroinDisabled;

        StopCurrentLoop();
    }

    // ==== Event-Handler ====
    private void OnCokeEnabled(object s, EventArgs e) => StartSpawn(DrugType.Coke);
    private void OnCokeDisabled(object s, EventArgs e) => StartDefault();     

    private void OnHazeEnabled(object s, EventArgs e) => StartSpawn(DrugType.Haze);
    private void OnHazeDisabled(object s, EventArgs e) => StartDefault();     

    private void OnLSDEenabled(object s, EventArgs e) => StartSpawn(DrugType.LSD);
    private void OnLSDDisabled(object s, EventArgs e) => StartDefault();    

    private void OnHeroinEnabled(object s, EventArgs e) => StartSpawn(DrugType.Heroin);
    private void OnHeroinDisabled(object s, EventArgs e) => StartDefault();   

    // ==== Steuerung ====
    private void StartDefault() => StartSpawn(defaultType);                 

    private void StartSpawn(DrugType type)
    {
        StopCurrentLoop();
        currentDrug = type;
        currentSpawnLoop = StartCoroutine(SpawnLoop(type));
    }

    private void StopCurrentLoop()
    {
        if (currentSpawnLoop != null)
        {
            StopCoroutine(currentSpawnLoop);
            currentSpawnLoop = null;
        }
        currentDrug = null;
    }

    private IEnumerator SpawnLoop(DrugType type)
    {
        while (true)
        {
            SpawnOne(type);
            yield return new WaitForSeconds(UnityEngine.Random.Range(spawnEveryMin, spawnEveryMax));
        }
    }

    private void SpawnOne(DrugType type)
    {
        var go = pooler.Get(type, out var prefabIndex);
        if (go == null) return; 

        Vector3 localPos = new Vector3(
            UnityEngine.Random.Range(-areaSize.x * 0.5f, areaSize.x * 0.5f),
            UnityEngine.Random.Range(0f, areaSize.y),
            0f
        );
        var worldPos = transform.TransformPoint(localPos);

        go.transform.SetParent(null);
        go.transform.position = worldPos;
        go.transform.rotation = Quaternion.identity;

        var po = go.GetComponent<DrugObstacle>();
        if (po == null) po = go.AddComponent<DrugObstacle>();
        po.pooler = pooler;
        po.drugType = type;
        po.prefabIndex = prefabIndex;

        var rb = go.GetComponent<Rigidbody>() ?? go.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;                          
        rb.detectCollisions = true;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (go.TryGetComponent<Collider>(out var col))
        {
            col.enabled = true;
            col.isTrigger = false;
        }

        go.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize.x, areaSize.y, 0.1f));
        Gizmos.matrix = Matrix4x4.identity;
    }
}
