using UnityEngine;

public class TurnLights : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    public bool lightsOn { get; private set; }
    private bool playerInside;

    void Start()
    {
        lightsOn = false;
        SetLights(false);
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            lightsOn = !lightsOn;
            SetLights(lightsOn);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = false;
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
