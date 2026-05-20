
using System;
using Unity.Netcode;
using UnityEngine;

public class TurnLights : NetworkBehaviour
{
    [SerializeField] private Light[] lights;
    [SerializeField] InteractablePanel codePanel;
    [SerializeField] GameObject historyObject;
    private AudioSource turnSound;
    public bool lightsOn { get; private set; }
    private bool playerInside;

    [SerializeField] InteractionEmission boxEmission;
    [SerializeField] InteractionEmission boxDoorEmission;

    private NetworkVariable<bool> lightsActivation = new NetworkVariable<bool>(true, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void Start()
    {
        turnSound = GetComponent<AudioSource>();
        codePanel.DeActivateEmission();
        codePanel.enabled = false;
        lightsOn = false;
        lightsActivation.OnValueChanged += SetLights;

        if (!IsServer) return;
        lightsActivation.Value = false;
    }

    

    void Update()
    {
        if (!IsServer) return;

        if (playerInside && Input.GetKeyDown(KeyCode.E))
        {
            lightsOn = !lightsOn;
            codePanel.enabled = lightsOn;
            lightsActivation.Value = lightsOn;
            playSoundClientRpc();

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

    private void SetLights(bool previousValue, bool newValue)
    {
        foreach (Light light in lights)
        {
            light.enabled = newValue;
        }
    }

    [ClientRpc]
    private void playSoundClientRpc()
    {
        turnSound.Play();
    }

}
