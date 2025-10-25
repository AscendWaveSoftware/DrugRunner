using UnityEngine;


public class DrugsHotKeys : MonoBehaviour
{
    [ContextMenu("Use Haze Drug (Test)")]
    public void TestHazeDrug()
    {
        DrugsManager.Instance.TryUse(DrugType.Haze);
    }

    [ContextMenu("Deactivate Haze Drug (Test)")]
    public void DTestHazeDrug()
    {
        DrugsManager.Instance.Stop(DrugType.Haze);
    }
}
