using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : NetworkBehaviour
{
    //check
    private bool isPaused;

    //ui canva
    [SerializeField] GameObject pauseCanva;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        //check if the player pressed the escape key
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            MenuManager();
        }
    }

    //stop or resume the game
    public void MenuManager()
    {
        //show the options panel and stop the game
        if (!isPaused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            pauseCanva.SetActive(true);
        }
        else
        {
            //resume all
            isPaused = false;
            Time.timeScale = 1.0f;
            pauseCanva.SetActive(false);
        }
    }

    //close the app and the connections
    public void CloseGame()
    {
        if (IsServer)
        {
            ulong clientId = 0;
            foreach (ulong id in NetworkManager.Singleton.ConnectedClientsIds)
            {
                if (id != NetworkManager.ServerClientId)
                {
                    clientId = id;
                }
            }

            NetworkManager.Singleton.DisconnectClient(clientId);

        }
        Application.Quit();
    }
}
