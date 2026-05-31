using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    //values
    [SerializeField] float speed = 2f;
    TextMeshProUGUI text;
    float alpha;


    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    //makes the text breathe
    void Update()
    {
        alpha = Mathf.PingPong(Time.time * speed, 1);
        text.alpha = alpha;
    }
}
