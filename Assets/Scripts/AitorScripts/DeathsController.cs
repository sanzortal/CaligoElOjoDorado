using UnityEngine;
using UnityEngine.UI;

public class DeathsController:MonoBehaviour
{
    [SerializeField] Canvas blackPanel;
    [SerializeField] Transform respawnPoint;

    private void Start()
    {
        blackPanel.enabled = false;
    }

    public Transform ActivatePanel()
    {
        blackPanel.enabled = true;
        return respawnPoint;
    }

    public void DeactivatePanel() { 
        blackPanel.enabled = false;
    }
}
