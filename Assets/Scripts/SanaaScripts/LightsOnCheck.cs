using UnityEngine;

public class LightsOnCheck : MonoBehaviour
{
    
    [SerializeField] TurnLights powerBox;
    [SerializeField] HistorySystem noLightDialogue;

    bool alreadyShown = false;

    //player inside check
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        //if the lights are deactivated show the text
        if (!powerBox.lightsOn && !alreadyShown)
        {
            alreadyShown = true;
            noLightDialogue.gameObject.SetActive(true);
        }
    }

}
