using NUnit.Framework.Constraints;
using UnityEngine;

public class SceneInteractableBehaviour : MonoBehaviour
{
    [SerializeField] PlayerController.emotions emotionNeeded;
    [SerializeField] bool isMovable;
    private bool isOpen;

    private void Start()
    {
        isOpen = false;
    }
    public void Open(PlayerController.emotions playerEmotion)
    {
        if (playerEmotion == emotionNeeded && !isMovable)
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

    public void Move(Vector3 moveDirection, float speed, PlayerController.emotions playerEmotion)
    {
        if (playerEmotion == emotionNeeded && isMovable)
        {
            this.transform.position = transform.position + moveDirection * speed * Time.deltaTime;
        }
    }
}
