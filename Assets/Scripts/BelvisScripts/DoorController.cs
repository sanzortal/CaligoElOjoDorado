using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour
{
    [SerializeField] Color doorColor;

    private Renderer render;



    void Start()
    {
        render = GetComponent<Renderer>();
    }

    
    void Update()
    {
        
    }

    public void DoorOpen()
    {
        render.material.color = doorColor;
    }
}
