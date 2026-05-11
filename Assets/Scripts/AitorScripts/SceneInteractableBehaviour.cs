using NUnit.Framework.Constraints;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneInteractableBehaviour : InteractionEmission
{
    [SerializeField] PlayerController.emotions emotionNeeded;
    [SerializeField] bool isMovable;
    private AudioSource moveAudio;

    private void Start()
    {
        moveAudio = GetComponent<AudioSource>();
        SetMaterials();
    }


    public void Move(GameObject parent, PlayerController.emotions playerEmotion)
    {
        if (playerEmotion == emotionNeeded && isMovable)
        {
            if (!NetworkManager.Singleton.IsServer) return;

            GetComponent<NetworkObject>().TrySetParent(parent.transform);

            DeActivateEmission();
        }
    }


    public void ClearParent()
    {
        if (!NetworkManager.Singleton.IsServer) return;

        GetComponent<NetworkObject>().TryRemoveParent();
    }

    public void playSound()
    {
        if (moveAudio != null && !moveAudio.isPlaying)
        {
            moveAudio.Play();
        }
    }

    public void stopSound()
    {
        if (moveAudio != null && moveAudio.isPlaying)
        {
            moveAudio.Stop();
        }
    }
}
