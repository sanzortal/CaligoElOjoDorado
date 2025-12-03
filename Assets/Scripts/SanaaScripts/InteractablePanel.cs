using UnityEngine;
using UnityEngine.InputSystem;

public class InteractablePanel : MonoBehaviour
{
    [SerializeField] GameObject interactablePanel;
    [SerializeField] Key interactKey;
    bool interacting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        OpenPanel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = false;
        }
    }

    public void OpenPanel()
    {
        if (Keyboard.current[interactKey].wasPressedThisFrame && interacting)
        {
            interactablePanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        interactablePanel.SetActive(false);
    }
}
