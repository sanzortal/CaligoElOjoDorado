using System.Collections;
using TMPro;
using UnityEngine;

public class ShowControls : MonoBehaviour
{
    //UI objects
    [SerializeField] GameObject controlsCanva;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string controlText;
    private bool activated;
    private Animator animator;

    //hide the canva and get values
    private void Start()
    {
        controlsCanva.SetActive(false);
        activated = false;
        animator = controlsCanva.GetComponent<Animator>();
    }

    //if one of the players collide with this object, activate the controls canva and start a timer
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

    //activate, wait, and deactivate the canva that shows the controls
    private IEnumerator ShowAndHide()
    {
        animator.SetTrigger("Show");

        yield return new WaitForSeconds(1.5f);

        animator.SetTrigger("Hide");

        this.gameObject.SetActive(false);
    }
}
