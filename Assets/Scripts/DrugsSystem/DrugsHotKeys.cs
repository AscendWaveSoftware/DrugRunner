using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class DrugsHotKeys : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;

    private InputAction drug1;
    private InputAction drug2;
    private InputAction drug3;
    private InputAction drug4;

    private void OnEnable()
    {
        var map = inputActions.FindActionMap("Player", true);

        drug1 = map.FindAction("Drug1", true);
        drug2 = map.FindAction("Drug2", true);
        drug3 = map.FindAction("Drug3", true);
        drug4 = map.FindAction("Drug4", true);

        drug1.performed += OnDrug1;
        drug2.performed += OnDrug2;
        drug3.performed += OnDrug3;
        drug4.performed += OnDrug4;

        inputActions.Enable();
    }

    private void OnDrug1(InputAction.CallbackContext ctx) =>
       DrugsManager.Instance.TryUse(DrugType.Haze);

    private void OnDrug2(InputAction.CallbackContext ctx) =>
        DrugsManager.Instance.TryUse(DrugType.LSD);

    private void OnDrug3(InputAction.CallbackContext ctx) =>
        DrugsManager.Instance.TryUse(DrugType.Coke);

    private void OnDrug4(InputAction.CallbackContext ctx) =>
        DrugsManager.Instance.TryUse(DrugType.Heroin);
}
