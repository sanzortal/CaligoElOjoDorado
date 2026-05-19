using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractablePanel : InteractionEmission
{
    [SerializeField] GameObject interactablePanel;
    [SerializeField] Key interactKey;
    private bool interacting = false;

    //player
    private PlayerController playerController;

    //code
    [SerializeField] GameObject door;
    private AudioSource doorSound;
    private Animation doorAnimation;
    [SerializeField] TMP_InputField textField;
    [SerializeField] string correctCode;
    [SerializeField] Light redLight;
    [SerializeField] Light greenLight;
    private bool done;
    private string playerAnswer;
    private AudioSource[] audios;
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        done = false;
        playerAnswer = "";
        audios = this.gameObject.GetComponents<AudioSource>();
        

        doorSound = door.GetComponent<AudioSource>();
        doorAnimation = door.GetComponent<Animation>();
        SetMaterials();
        
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        redLight.enabled = false;
        greenLight.enabled = false;
    }
   
    void Update()
    {
        if (!IsServer) return;
        OpenPanel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = true;
            playerController = collision.gameObject.GetComponent<PlayerController>();

            if (!done)
            {
                ActivateEmission();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!IsServer) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = false;
            playerController = null;

            if (!done)
            {
                DeActivateEmission();
            }
        }
    }

    public void OpenPanel()
    {
        if (Keyboard.current[interactKey].wasPressedThisFrame && interacting && !done)
        {
            interactablePanel.SetActive(true);
            playerController.enabled = false;

        }
    }

    public void ClosePanel()
    {
        interactablePanel.SetActive(false);
        playerAnswer = "";
        SetText();
        playerController.enabled = true;
    }

    public void AddNumber(string number)
    {
        if (playerAnswer.Length <= 3)
        {
            playerAnswer += number;
            SetText();
        }
    }

    public void DeleteNumber()
    {
        if (playerAnswer.Length > 0)
        {
            playerAnswer = playerAnswer.Substring(0, playerAnswer.Length - 1);
            SetText();
        }
    }


    public void Confirm()
    {
        if (playerAnswer.Equals(correctCode))
        {
            done = true;
            DeActivateEmission();
            CorrectAnsClientRpc();
            ClosePanel();
        }
        else
        {
            InCorrectAnsClientRpc();
        }
    }

    [ClientRpc]
    private void CorrectAnsClientRpc()
    {
        doorSound.Play();
        doorAnimation.Play("Door|Open");

        redLight.enabled = false;
        greenLight.enabled = true;
        audios[0].Play();
    }

    [ClientRpc]
    private void InCorrectAnsClientRpc()
    {
        if (!redLight.enabled)
        {
            redLight.enabled = true;
        }
        audios[1].Play();
    }

    public void SetText()
    {
        textField.text = playerAnswer;
    }
}
