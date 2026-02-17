using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    TextMeshProUGUI text;
    float alpha;


    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    
    void Update()
    {
        alpha = Mathf.PingPong(Time.time * speed, 1);
        text.alpha = alpha;
    }
}
