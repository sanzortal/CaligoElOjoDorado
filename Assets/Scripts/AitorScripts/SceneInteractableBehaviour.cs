using NUnit.Framework.Constraints;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneInteractableBehaviour : InteractionEmission
{
    //checks
    [SerializeField] PlayerController.emotions emotionNeeded;
    [SerializeField] bool isMovable;

    private AudioSource moveAudio;

    //get values and deactivate emission materials
    private void Start()
    {
        moveAudio = GetComponent<AudioSource>();
        SetMaterials();
        DeActivateEmission();
    }

    //if the object is movable and the player is in the correct emotion, set the player as the parent
    public void Move(GameObject parent, PlayerController.emotions playerEmotion)
    {
        if (playerEmotion == emotionNeeded && isMovable)
        {
            if (!NetworkManager.Singleton.IsServer) return;

            GetComponent<NetworkObject>().TrySetParent(parent.transform);

            DeActivateEmission();
        }
    }

    //remove the player as the parent of the object
    public void ClearParent()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        GetComponent<NetworkObject>().TryRemoveParent();
    }

    //play a sound also for all the clients
    [ClientRpc]
    public void playSoundClientRpc()
    {
        if (moveAudio != null && !moveAudio.isPlaying)
        {
            moveAudio.Play();
        }
    }

    //stop the sound also for all the clients
    [ClientRpc]
    public void stopSoundClientRpc()
    {
        if (moveAudio != null && moveAudio.isPlaying)
        {
            moveAudio.Stop();
        }
    }
}
