using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MenuFunc : MonoBehaviour
{
    // Start is called before the first frame update

    
    public Button Quit;
    public Button StartButton;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider Volume;
    
    void Start()
    {
        //pInput = GameObject.Find("Player").GetComponent<GamepadInput>();

        if (!PlayerPrefs.HasKey("Volume"))
        {
            PlayerPrefs.SetFloat("Volume", 1);
            Load();
        } else
        {
            Load();
        }
    }

    public void changeVolume()
    {
        AudioListener.volume = Volume.value;
        print(AudioListener.volume);
        Save();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            TogglePauseMenu();
        }
    }

    public void  QuitGame()
    {
        SceneManager.LoadScene(0);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    private void TogglePauseMenu()
    {
        if(pauseMenuUI != null)
        {
            bool isPaused = pauseMenuUI.activeSelf;
            pauseMenuUI.SetActive(!isPaused);

            if (!isPaused)
            {
                Time.timeScale = 0; // Pause the game
            }
            else
            {
                Time.timeScale = 1; // Resume the game
            }
        }
       
    }


    private void Load()
    {
        Volume.value = PlayerPrefs.GetFloat("Volume");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("Volume", Volume.value);
    }
}

