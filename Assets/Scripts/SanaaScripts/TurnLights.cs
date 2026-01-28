using UnityEngine;

public class TurnLights : MonoBehaviour
{
    [SerializeField] private Light[] lights;
    public bool lightsOn { get; private set; }

    void Start()
    {
        lightsOn = false;
        SetLights(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                lightsOn = !lightsOn;
                SetLights(lightsOn);
            }
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
