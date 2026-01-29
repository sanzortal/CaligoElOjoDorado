using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathsController:MonoBehaviour
{
    private Animator animatorController;
    [SerializeField] Transform respawnPoint;

    private void Start()
    {
        animatorController = this.gameObject.GetComponentInChildren<Animator>();
    }

    public Transform ActivatePanel()
    {
        animatorController.SetTrigger("Death");
        return respawnPoint;
    }

    public void DeactivatePanel() {
        animatorController.SetTrigger("Respawn");
    }

}
