
using System.Collections;
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
        //textAnimator = textAdvice.gameObject.GetComponent<Animator>();
    }

    public static Transform ActivatePanel()
    {
        instance.animatorController.SetTrigger("Death");
        instance.CanvaCircle.SetActive(true);
        instance.isActive = true;
        instance.StartCoroutine(instance.FadeText());
        return instance.respawnPoint;
    }

    public static void DeactivatePanel() {
        instance.animatorController.SetTrigger("Respawn");
        instance.CanvaCircle.SetActive(false);
        instance.isActive = false;
    }

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

    public static void RespawnAll()
    {
        if (instance.OnPlayerDeath != null)
        {
            instance.OnPlayerDeath();
        }
    }

    public static void ChangeRespawnPoint(Transform newPoint)
    {
        instance.respawnPoint = newPoint;
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
