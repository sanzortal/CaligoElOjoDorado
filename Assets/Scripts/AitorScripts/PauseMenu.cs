using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    [SerializeField] GameObject pauseCanva;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            MenuManager();
        }
    }

    public void MenuManager()
    {
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseCanva.SetActive(true);
        }
        else
        {
            isPaused = false;
            Time.timeScale = 1.0f;
            pauseCanva.SetActive(false);
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
