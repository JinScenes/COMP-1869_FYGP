using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class DeathCanvasScript : MonoBehaviour
{
    public Button retryButton;
    public Button quitButton;

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
