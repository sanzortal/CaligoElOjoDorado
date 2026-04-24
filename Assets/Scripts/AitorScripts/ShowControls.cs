using System.Collections;
using TMPro;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    [SerializeField] GameObject controlsCanva;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string controlText;
    private bool activated;
    private Animator animator;
    private void Start()
    {
        controlsCanva.SetActive(false);
        activated = false;
        animator = controlsCanva.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!activated)
        {
            activated = true;
            controlsCanva.SetActive(true);
            text.text = controlText;

            StartCoroutine(ShowAndHide());
        }
    }

    private IEnumerator ShowAndHide()
    {
        animator.SetTrigger("Show");

        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Hide");

        this.gameObject.SetActive(false);
    }
}
