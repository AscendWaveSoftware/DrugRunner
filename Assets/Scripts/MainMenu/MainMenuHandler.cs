using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject controlsPanel;

    private string playScene = "PlayScene";
    private bool controlsActive;

    private void Awake()
    {
        startButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(playScene);
        });

        controlsButton.onClick.AddListener(() =>
        {
            controlsActive = !controlsActive;

            if (controlsActive)
                controlsPanel.SetActive(true);
            else
                controlsPanel.SetActive(false);
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void Start()
    {
        controlsPanel.SetActive(false);
        controlsActive = false;
    }
}
