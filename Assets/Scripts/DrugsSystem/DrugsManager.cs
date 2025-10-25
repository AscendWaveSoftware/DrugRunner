using System.Collections.Generic;
using UnityEngine;

public class DrugsManager : MonoBehaviour
{
    public static DrugsManager Instance { get; private set; }

    [Header("Scene References")]
    [SerializeField] private PlayerEffects player;
    [SerializeField] private VisualEffectsController vfx;

    private DrugsContext ctx;
    private readonly Dictionary<DrugType, IDrug> allDrugs = new();
    private readonly HashSet<DrugType> consumedThisRun = new();
    private readonly HashSet<DrugType> active = new();

    private DrugType? currentActive = null;

    public bool IsAnyDrugActive => currentActive.HasValue;
    public DrugType? CurrentActive => currentActive;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        ctx = new DrugsContext(this, player, vfx);

        Register(new HazeDrug(_duration: 5f));
        Register(new LSDDrug(_duration: 10f));
        Register(new CokeDrug(_duration: 10f));
        Register(new HeroinDrug(_duration: 10f));
        //TODO: Andere Drugs implementieren
    }

    private void Start()
    {
        ResetRunLocks();
    }

    public void Register(IDrug _drug)
    {
        allDrugs[_drug.Type] = _drug;
    }

    public bool TryUse(DrugType _type)
    {
        if (consumedThisRun.Contains(_type))
            return false;

        if (currentActive.HasValue)
            return false;

        if (!allDrugs.TryGetValue(_type, out var drug))
            return false;

        consumedThisRun.Add(_type);
        drug.Begin(ctx);
        active.Add(_type);
        currentActive = _type;
        return true;
    }

    public void Stop(DrugType _type)
    {
        if (!active.Contains(_type))
            return;

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

    /// <summary>
    /// Call bei neuem Run.
    /// </summary>
    public void ResetRunLocks()
    {
        StopAllActives();
        consumedThisRun.Clear(); 
    }
}
