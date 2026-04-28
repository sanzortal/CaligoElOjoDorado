using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractionEmission : MonoBehaviour
{
    private Material[] materials;

    private void Start()
    {
        SetMaterials();
    }
    public void SetMaterials()
    {
        materials = this.gameObject.GetComponent<Renderer>().materials;
    }
    public void ActivateEmission()
    {
        foreach (Material material in materials)
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    public void DeActivateEmission()
    {
        foreach (Material material in materials)
        {
            material.DisableKeyword("_EMISSION");
        }
    }
}
