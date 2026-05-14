using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : NetworkBehaviour
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
