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

<<<<<<< Updated upstream
    public  void QuitGame()
=======
    public void QuitGame()
>>>>>>> Stashed changes
    {
        Application.Quit();
    }
}
