using UnityEngine;

public class WallActivation : MonoBehaviour
{
    [SerializeField] GameObject wall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       wall.SetActive(false); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            wall.SetActive(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            wall.SetActive(false);
        }
    }
}
