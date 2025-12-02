using UnityEngine;
using UnityEngine.InputSystem;

public class InteractuableObjects : MonoBehaviour
{

    private bool active = false;

    private Color initColor;

    [SerializeField] Color activeColor;

    private bool touchPlayer = false;

    [SerializeField] Key interactionKey;

    private Renderer render;

    [SerializeField] PuzzleManager manager;


  

    void Start()
    {
        render = GetComponent<Renderer>();

        initColor = render.material.color;
    }

    void Update()
    {
        if (touchPlayer && Keyboard.current[interactionKey].wasPressedThisFrame && !active)
        {
            PressButton();
            if (manager != null)
                manager.ButtonChecker();
        }
    }

    void PressButton()
    {
        render.material.color = activeColor;
        active = true;
    }


    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            touchPlayer = true;
        }

    }

    public bool Active()
    {
        return active;
    }


    public void ResetButtons()
    {
        active = false;
        render.material.color = initColor;
    }
}
