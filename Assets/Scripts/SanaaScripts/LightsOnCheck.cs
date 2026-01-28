using UnityEngine;

public class LightsOnCheck : MonoBehaviour
{
    [SerializeField] TurnLights powerBox;
    [SerializeField] HistorySystem noLightDialogue;

    bool alreadyShown = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        //no hay luz
        if (!powerBox.lightsOn && !alreadyShown)
        {
            alreadyShown = true;
            noLightDialogue.gameObject.SetActive(true);
        }
    }

}
