using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DrugsManager : MonoBehaviour
{
    public static DrugsManager Instance { get; private set; }

    [Header("Scene References")]
    [SerializeField] private VisualEffectsController vfx;

    private DrugsContext ctx;
    private readonly Dictionary<DrugType, IDrug> allDrugs = new();
    private readonly HashSet<DrugType> consumedThisRun = new();
    private readonly HashSet<DrugType> active = new();

    private DrugType? currentActive = null;

    public bool IsAnyDrugActive => currentActive.HasValue;
    public DrugType? CurrentActive => currentActive;

    public static bool playerCanDie = true;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

     
        SceneManager.sceneLoaded += OnSceneLoaded;

        RebindSceneRefs();              
        BuildDrugRegistry();               
        HardResetTime();              
        ResetRunLocks();          
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       
        RebindSceneRefs();
        HardResetTime();
        ResetRunLocks();
    }

    private void RebindSceneRefs()
    {
        if (vfx == null) vfx = FindObjectOfType<VisualEffectsController>(true);
        ctx = new DrugsContext(this, vfx); 
    }

    private void HardResetTime()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }

    private void BuildDrugRegistry()
    {
        
        Register(new HazeDrug(_duration: 5f));
        Register(new LSDDrug(_duration: 10f));
        Register(new CokeDrug(_duration: 10f));
        Register(new HeroinDrug(_duration: 10f));
        // TODO: weitere Drugs hier registrieren
    }

    public void Register(IDrug _drug)
    {
        allDrugs[_drug.Type] = _drug;
    }

    public bool TryUse(DrugType _type)
    {
        if (consumedThisRun.Contains(_type)) return false;
        if (currentActive.HasValue) return false;
        if (!allDrugs.TryGetValue(_type, out var drug)) return false;

        consumedThisRun.Add(_type);
        drug.Begin(ctx);
        active.Add(_type);
        currentActive = _type;
        return true;
    }

    public void Stop(DrugType _type)
    {
        if (!active.Contains(_type)) return;

        var drug = allDrugs[_type];
        drug.End(ctx);
        active.Remove(_type);

        if (currentActive == _type)
            currentActive = null;
    }

    public void StopAllActives()
    {
        foreach (var t in new List<DrugType>(active))
            Stop(t);
    }

    /// <summary>Frischer Run: alle Effekte aus, Sperren löschen.</summary>
    public void ResetRunLocks()
    {
        StopAllActives();
        consumedThisRun.Clear();
        currentActive = null;
    }
}
