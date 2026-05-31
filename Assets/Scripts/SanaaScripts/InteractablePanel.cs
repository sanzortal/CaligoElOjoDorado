using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractablePanel : InteractionEmission
{
    //interaction objects
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

    //when the object spawn deactivate its lights
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

    //check if the player is inside and activate the emission materials
    private void OnCollisionEnter(Collision collision)
    {
        if (!IsServer) return;
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = true;

            //stores the player controller for other things
            playerController = collision.gameObject.GetComponent<PlayerController>();

            if (!done)
            {
                ActivateEmission();
            }
        }
    }

    //if the player leave the object, deactivate the emission materials
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

    //show the canva with the buttons if the player press the interact key
    public void OpenPanel()
    {
        if (Keyboard.current[interactKey].wasPressedThisFrame && interacting && !done)
        {
            interactablePanel.SetActive(true);
            playerController.enabled = false;

        }
    }

    //hide the canva with the buttons
    public void ClosePanel()
    {
        interactablePanel.SetActive(false);
        playerAnswer = "";
        SetText();
        playerController.enabled = true;
    }

    //add the number that the player presses if there are 3 digits or less
    public void AddNumber(string number)
    {
        if (playerAnswer.Length <= 3)
        {
            playerAnswer += number;
            SetText();
        }
    }

    //delete the last number from those shown
    public void DeleteNumber()
    {
        if (playerAnswer.Length > 0)
        {
            playerAnswer = playerAnswer.Substring(0, playerAnswer.Length - 1);
            SetText();
        }
    }

    //check if the answer of the player is the correct code or not
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

    //if the code is correct play some audios and open the door also for the clients 
    [ClientRpc]
    private void CorrectAnsClientRpc()
    {
        doorSound.Play();
        doorAnimation.Play("Door|Open");

        redLight.enabled = false;
        greenLight.enabled = true;
        audios[0].Play();
    }

    //if the code is incorrect play incorrect audio and show red light
    [ClientRpc]
    private void InCorrectAnsClientRpc()
    {
        if (!redLight.enabled)
        {
            redLight.enabled = true;
        }
        audios[1].Play();
    }

    //set the text that the player presses in real time
    public void SetText()
    {
        textField.text = playerAnswer;
    }
}
