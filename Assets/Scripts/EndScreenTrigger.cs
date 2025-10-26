using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreenTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            SceneManager.LoadScene("WinScene");
    }
}
