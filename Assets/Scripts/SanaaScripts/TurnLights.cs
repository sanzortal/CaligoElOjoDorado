using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class TurnLights : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] InteractablePanel codePanel;
    [SerializeField] GameObject historyObject;
    private AudioSource turnSound;
    public bool lightsOn { get; private set; }
    private bool playerInside;

    [SerializeField] InteractionEmission boxEmission;
    [SerializeField] InteractionEmission boxDoorEmission;

    void Start()
    {
        turnSound = GetComponent<AudioSource>();
        codePanel.enabled = false;
        lightsOn = false;
        SetLights(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            lightsOn = !lightsOn;
            codePanel.enabled = lightsOn;
            SetLights(lightsOn);
            turnSound.Play();

            if (historyObject != null)
            {
                if (lightsOn)
                {
                    historyObject.SetActive(false);
                }
                else
                {
                    historyObject.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = true;
            boxEmission.ActivateEmission();
            boxDoorEmission.ActivateEmission();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = false;
            boxEmission.DeActivateEmission();
            boxDoorEmission.DeActivateEmission();
        }
    }

    void SetLights(bool state)
    {
        foreach (Light light in lights)
        {
            light.enabled = state;
        }
    }
}
