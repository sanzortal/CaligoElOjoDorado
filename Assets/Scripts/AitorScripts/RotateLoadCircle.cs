using UnityEngine;
using UnityEngine.UI;

public class RotateLoadCircle : MonoBehaviour
{
    [SerializeField] Image circle;
    [SerializeField] float speed;
    private RectTransform rectTransform;
    private void Start()
    {
        rectTransform = circle.GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        rectTransform.Rotate(0f,0f, -(Time.deltaTime * speed));
    }

    private void OnDisable()
    {
        rectTransform.eulerAngles = Vector3.zero;
    }
}
