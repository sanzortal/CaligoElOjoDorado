using UnityEngine;

[System.Serializable]
struct StateParameters
{
    [SerializeField]
    [Tooltip("Accion que se ha de cumplir para cambiar de estado")]
    private Action actionToCheck;

    [SerializeField]
    [Tooltip("Valor que tiene que devolver la accion")]
    private bool actionValue;

    [SerializeField]
    [Tooltip("Siguiente estado al que vamos a ir cuando la acción se cumpla")]
    private State nextState;

    public Action GetAction()
    {
        return actionToCheck;
    }

    public bool GetActionValue()
    {
        return actionValue;
    }

    public State GetNewState()
    {
        return nextState;
    }

}

public class State : ScriptableObject
{
    [SerializeField]
    private StateParameters[] stateParameters;

    public virtual State Run(GameObject owner)
    {
        foreach (StateParameters parameter in stateParameters)
        {
            if (parameter.GetAction().Check(owner) == parameter.GetActionValue())
            {
                return parameter.GetNewState();
            }
        }
        return null;
    }

    public void DrawAllGizmos(GameObject owner)
    {
        foreach (StateParameters parameter in stateParameters)
        {
            DrawableAction drawableAction = (DrawableAction)parameter.GetAction();

            if (drawableAction)
            {
                drawableAction.DrawGizmo(owner);
            }
        }
    }
}