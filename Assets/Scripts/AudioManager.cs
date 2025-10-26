using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    private void OnEnable()
    {
        DrugBase.OnAnimationPlayed += DrugBase_OnAnimationPlayed;
    }

    private void OnDisable()
    {
        DrugBase.OnAnimationPlayed -= DrugBase_OnAnimationPlayed;
    }

    private void DrugBase_OnAnimationPlayed(object sender, System.EventArgs e)
    {
        source.Stop();
        source.Play();
    }
}
