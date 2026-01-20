using UnityEngine;

public abstract class Action : ScriptableObject
{
    public abstract bool Check(GameObject owner);      

}
