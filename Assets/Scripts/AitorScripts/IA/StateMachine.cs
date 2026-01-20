using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private State entryPoint;

    private State currentState;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = entryPoint;
    }

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    void RunStateMachine()
    {
        State nexState = currentState.Run(gameObject);

        if (nexState != null)
        {
            currentState = nexState;
        }
    }

    private void OnDrawGizmos()
    {
        if (currentState)
        {
            currentState.DrawAllGizmos(gameObject);
        }
        else if (entryPoint)
        {
            entryPoint.DrawAllGizmos(gameObject);
        }
    }
}
