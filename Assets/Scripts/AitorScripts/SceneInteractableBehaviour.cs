using NUnit.Framework.Constraints;
using UnityEngine;

public class SceneInteractableBehaviour : MonoBehaviour
{
    [SerializeField] PlayerController.emotions emotionNeeded;
    private bool isOpen;

    private void Start()
    {
        isOpen = false;
    }
    public void Interact(PlayerController.emotions playerEmotion)
    {
        if (playerEmotion == emotionNeeded)
        {
            if (!isOpen)
            {
                this.transform.position = new Vector3(this.transform.position.x + 4, this.transform.position.y, this.transform.position.z);
                isOpen = true;
            }
            else
            {
                this.transform.position = new Vector3(this.transform.position.x - 4, this.transform.position.y, this.transform.position.z);
                isOpen = false;
            }
        }
    }
}
