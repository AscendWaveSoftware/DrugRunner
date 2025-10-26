using UnityEngine;

public class DrugEffectScreen : MonoBehaviour
{
    [SerializeField] private Animator animator;


    private void Start()
    {
        DrugBase.OnAnimationPlayed += DrugBase_OnAnimationPlayed;
        DrugBase.OnAnimationStoped += DrugBase_OnAnimationStoped;
    }


    private void DrugBase_OnAnimationPlayed(object sender, System.EventArgs e)
    {
        animator.ResetTrigger("Active");
        animator.SetTrigger("Active");
    }

    private void DrugBase_OnAnimationStoped(object sender, System.EventArgs e)
    {
        animator.ResetTrigger("Active");
    }

}
