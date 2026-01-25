using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathsController:MonoBehaviour
{
    private Animator animatorController;
    [SerializeField] Canvas fadeCanvas;
    [SerializeField] Transform respawnPoint;

    private void Start()
    {
        fadeCanvas.enabled = false;

        animatorController = this.gameObject.GetComponentInChildren<Animator>();
    }

    public Transform ActivatePanel()
    {
        fadeCanvas.enabled = true;
        animatorController.Play("FadeInDeath", 0, 0f);
        return respawnPoint;
    }

    public void DeactivatePanel() {
        animatorController.SetTrigger("Death");
    }

}
