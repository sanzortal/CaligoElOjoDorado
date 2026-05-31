using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class HistorySystem : NetworkBehaviour
{
    //objects needed to show the history 
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField, TextArea(4,6)] string[] dialogueLines;
    [SerializeField] float typingSpeed = 0.05f;

    //checks
    bool dialogueStarted = false;
    int currentLine = 0;
    bool isTyping = false;

    PlayerController playerController;



    // Update is called once per frame
    void Update()
    {
        //check if the text is being shown and the player presses the space
        if (dialogueStarted && Input.GetKeyDown(KeyCode.Space))
        {
            //if the text is being shown stop all the movement and show all of it
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                isTyping = false;
            }
            else
            {
                //if the text is already shown starts the new line or end the dialogie
                currentLine++;
                if (currentLine < dialogueLines.Length)
                {
                    StartCoroutine(TypeLine(dialogueLines[currentLine]));
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    //if the player enters for the first time shows the history
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dialogueStarted)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            dialogueStarted = true;

            playerController = collision.GetComponent <PlayerController>();


            //deactivate the player movement
            if (playerController != null)
                playerController.enabled = false;

            //show the text
            dialoguePanel.SetActive(true);
            currentLine = 0;
            StartCoroutine(TypeLine(dialogueLines[currentLine]));

            //play idle animation
            collision.GetComponentInChildren<Animator>().Play("Armature|Idle");

        }
    }

    //show the text letter by letter
    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    //activate the player movement and finish the dialogue
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        //reactivate the player movement
        if (playerController != null)
            playerController.enabled = true;

        if (!NetworkManager.Singleton.IsServer) return;

        //hide the text
        GetComponent<NetworkObject>().Despawn(true);
    }

}
