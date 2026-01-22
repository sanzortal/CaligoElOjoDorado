using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneInteractableBehaviour : MonoBehaviour
{
    [SerializeField] PlayerController.emotions emotionNeeded;
    [SerializeField] bool isMovable;
    private bool isOpen;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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

    public void Move(GameObject parent, PlayerController.emotions playerEmotion)
    {
        if (playerEmotion == emotionNeeded && isMovable)
        {
            this.transform.parent = parent.transform;
        }
    }

    public void ClearParent()
    {
        this.transform.parent = null;
    }
}
