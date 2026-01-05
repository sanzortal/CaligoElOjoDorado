using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractablePanel : MonoBehaviour
{
    [SerializeField] GameObject interactablePanel;
    [SerializeField] Key interactKey;
    private bool interacting = false;

    //player
    private PlayerController playerController;

    //code
    [SerializeField] GameObject door;
    [SerializeField] TMP_InputField textField;
    [SerializeField] string correctCode;
    [SerializeField] Light redLight;
    [SerializeField] Light greenLight;
    private bool done;
    private string playerAnswer;
    private AudioSource[] audios;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        done = false;
        playerAnswer = "";
        audios = this.gameObject.GetComponents<AudioSource>();
        redLight.enabled = false;
        greenLight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        OpenPanel();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = true;
            playerController = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interacting = false;
            playerController = null;
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

            door.transform.position = new Vector3(29.14f, 2.771813f, 0.6f);
            door.transform.eulerAngles = new Vector3(0f, 270f, 0f);

            redLight.enabled = false;
            greenLight.enabled = true;
            audios[0].Play();
            ClosePanel();
        }
        else
        {
            if (!redLight.enabled)
            {
                redLight.enabled = true;
            }
            audios[1].Play();
        }
    }

    public void SetText()
    {
        textField.text = playerAnswer;
    }
}
