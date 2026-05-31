
using System.Collections;
using Unity.Networking.Transport.Error;
using UnityEngine;
using UnityEngine.UI;

public delegate void SimpleDelegate();
public class DeathsController:MonoBehaviour
{
    private Animator animatorController;
    [SerializeField] Transform respawnPoint;
    static DeathsController instance;

    [SerializeField] GameObject CanvaCircle;

    [SerializeField] Text textAdvice;
    [SerializeField] Animator textAnimator;
    [SerializeField] string[] advices;

    private bool isActive;

    event SimpleDelegate OnPlayerDeath;

    //singleton
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
        isActive = false;
        animatorController = this.gameObject.GetComponentInChildren<Animator>();
        
    }

    //activate the death panel and starts the coroutines
    public static void ActivatePanel()
    {
        instance.animatorController.SetTrigger("Death");
        instance.CanvaCircle.SetActive(true);
        instance.isActive = true;
        instance.StartCoroutine(instance.FadeText());
        
    }

    //return the point that the player needs to use to respawn
    public static Transform ReturnRespawnPoint()
    {
        return instance.respawnPoint;
    }

    //hide the death panel and stop all of coroutines
    public static void DeactivatePanel() {
        instance.animatorController.SetTrigger("Respawn");
        instance.CanvaCircle.SetActive(false);
        instance.isActive = false;
    }

    //make the text breathe
    public IEnumerator FadeText()
    {
        while (isActive)
        {
            int randomNum = Random.Range(0, advices.Length);
            textAdvice.text = advices[randomNum];
            textAnimator.SetTrigger("Show");

            yield return new WaitForSeconds(1.5f);
            textAnimator.SetTrigger("Hide");

            yield return new WaitForSeconds(1f);

        }

    }

    //respawn all the objects that are subscribed to the event
    public static void RespawnAll()
    {
        if (instance.OnPlayerDeath != null)
        {
            instance.OnPlayerDeath();
        }
    }

    //change the point that the player has tu use to respawn
    public static void ChangeRespawnPoint(Transform newPoint)
    {
        instance.respawnPoint = newPoint;
    }

    //register in the death function
    public static void RegisterOnPlayerDeath(SimpleDelegate respawn)
    {
        instance.OnPlayerDeath += respawn;
    }

    //unregister in the death function
    public static void UnRegisterOnPlayerDeath(SimpleDelegate respawn)
    {
        instance.OnPlayerDeath -= respawn;
    }

}
