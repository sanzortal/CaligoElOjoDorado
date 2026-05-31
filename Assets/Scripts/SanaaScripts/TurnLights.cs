
using System;
using Unity.Netcode;
using UnityEngine;

public class TurnLights : NetworkBehaviour
{
    //objects to "modify"
    [SerializeField] private Light[] lights;
    [SerializeField] InteractablePanel codePanel;
    [SerializeField] GameObject historyObject;
    private AudioSource turnSound;

    //checks
    public bool lightsOn { get; private set; }
    private bool playerInside;

    //emissions
    [SerializeField] InteractionEmission boxEmission;
    [SerializeField] InteractionEmission boxDoorEmission;

    //network check
    private NetworkVariable<bool> lightsActivation = new NetworkVariable<bool>(false, 
            NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    void Start()
    {
        turnSound = GetComponent<AudioSource>();
        codePanel.DeActivateEmission();
        codePanel.enabled = false;
        lightsOn = false;
        lightsActivation.OnValueChanged += SetLights;

    }

    

    void Update()
    {
        if (!IsServer) return;

        //activate or desactivate the lights and advice text
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

    //player inside check
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = true;
            boxEmission.ActivateEmission();
            boxDoorEmission.ActivateEmission();
        }
    }

    //player outside check
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            playerInside = false;
            boxEmission.DeActivateEmission();
            boxDoorEmission.DeActivateEmission();
        }
    }

    //network activation
    private void SetLights(bool previousValue, bool newValue)
    {
        foreach (Light light in lights)
        {
            light.enabled = newValue;
        }
    }

    //play de sound in clients
    [ClientRpc]
    private void playSoundClientRpc()
    {
        turnSound.Play();
    }

}
