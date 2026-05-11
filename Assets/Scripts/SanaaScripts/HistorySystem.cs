using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class HistorySystem : NetworkBehaviour
{
    [SerializeField] GameObject dialoguePanel;
    [SerializeField] TMP_Text dialogueText;
    [SerializeField, TextArea(4,6)] string[] dialogueLines;
    [SerializeField] float typingSpeed = 0.05f;

    bool dialogueStarted = false;
    int currentLine = 0;
    bool isTyping = false;
    PlayerController playerController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dialogueStarted && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLine];
                isTyping = false;
            }
            else
            {
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

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !dialogueStarted)
        {
            if (!NetworkManager.Singleton.IsServer) return;
            dialogueStarted = true;

            playerController = collision.GetComponent <PlayerController>();


            // se desactiva el script de movimiento del jugador
            if (playerController != null)
                playerController.enabled = false;

            dialoguePanel.SetActive(true);
            currentLine = 0;
            StartCoroutine(TypeLine(dialogueLines[currentLine]));

            collision.GetComponentInChildren<Animator>().Play("Armature|Idle");

        }
    }

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
    void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        // se reactiva el movimiento del jugador
        if (playerController != null)
            playerController.enabled = true;

        if (!NetworkManager.Singleton.IsServer) return;

        GetComponent<NetworkObject>().Despawn(true);
    }

}
