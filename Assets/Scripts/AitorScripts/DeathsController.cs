using System.Collections;
using UnityEditor.Rendering.Universal.ShaderGUI;
using UnityEngine;
using UnityEngine.UI;

public delegate void SimpleDelegate();
public class DeathsController:MonoBehaviour
{
    private Animator animatorController;
    [SerializeField] Transform respawnPoint;
    static DeathsController instance;

    event SimpleDelegate OnPlayerDeath;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        animatorController = this.gameObject.GetComponentInChildren<Animator>();
    }

    public static Transform ActivatePanel()
    {
        instance.animatorController.SetTrigger("Death");
        return instance.respawnPoint;
    }

    public static void DeactivatePanel() {
        instance.animatorController.SetTrigger("Respawn");
    }

    public static void RespawnAll()
    {
        if (instance.OnPlayerDeath != null)
        {
            instance.OnPlayerDeath();
        }
    }

    public static void RegisterOnPlayerDeath(SimpleDelegate respawn)
    {
        instance.OnPlayerDeath += respawn;
    }
    public static void UnRegisterOnPlayerDeath(SimpleDelegate respawn)
    {
        instance.OnPlayerDeath -= respawn;
    }

}
