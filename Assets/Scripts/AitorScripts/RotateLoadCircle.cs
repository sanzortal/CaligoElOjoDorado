using UnityEngine;
using UnityEngine.UI;

public class RotateLoadCircle : MonoBehaviour
{
    //image 
    [SerializeField] Image circle;

    //speed
    [SerializeField] float speed;
    private RectTransform rectTransform;

    //get value and deactivate this object
    private void Start()
    {
        rectTransform = circle.GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }
    
    //rotate the circle
    void Update()
    {
        rectTransform.Rotate(0f,0f, -(Time.deltaTime * speed));
    }

    //reset the rotation
    private void OnDisable()
    {
        rectTransform.eulerAngles = Vector3.zero;
    }
}
