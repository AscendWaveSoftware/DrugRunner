using UnityEngine;

public class VisualEffectsController : MonoBehaviour
{
    [Header("Post Processing / Volumes")]
    public GameObject LSDVolume;
    public GameObject HazeVolume;
    public GameObject CokeVolume;
    public GameObject HeroinVolume;

    [Header("Map Toggles")]
    public GameObject[] objectsToRevealOnHaze;
    public GameObject[] objectsToRevealOnLSD;
    public GameObject[] objectsToRevealOnCoke;
    public GameObject[] objectsToRevealOnHeroin;

    /// <summary>
    /// Aktiviert Global Volume und Map Objects für LSD-Effekt.
    /// </summary>
    public void SetLSD(bool _on)
    {
        if (LSDVolume)
            LSDVolume.SetActive(_on);

        foreach (var go in objectsToRevealOnLSD)
            if (go)
                go.SetActive(_on);
    }

    /// <summary>
    /// Aktiviert Global Volume und Map Objects für Haze-Effekt.
    /// </summary>
    public void SetHaze(bool _on)
    {
        if (HazeVolume)
            HazeVolume.SetActive(_on);

        foreach (var go in objectsToRevealOnHaze)
            if (go)
                go.SetActive(_on);
    }

    /// <summary>
    /// Aktiviert Global Volume und Map Objects für Coke-Effekt.
    /// </summary>
    public void SetCoke(bool _on)
    {
        if (CokeVolume)
            CokeVolume.SetActive(_on);

        foreach (var go in objectsToRevealOnCoke)
            if (go)
                go.SetActive(_on);
    }

    /// <summary>
    /// Aktiviert Global Volume und Map Objects für Heroin-Effekt.
    /// </summary>
    public void SetHeroin(bool _on)
    {
        if (HeroinVolume)
            HeroinVolume.SetActive(_on);

        foreach (var go in objectsToRevealOnHeroin)
            if (go)
                go.SetActive(_on);
    }
}
