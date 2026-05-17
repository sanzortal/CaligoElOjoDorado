using Unity.Netcode;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractionEmission : NetworkBehaviour
{
    private Material[] materials;

    private void Start()
    {
        SetMaterials();
        DeActivateEmission();
    }
    public void SetMaterials()
    {
        materials = this.gameObject.GetComponent<Renderer>().materials;
    }
    public void ActivateEmission()
    {
        if (!IsHost) return;
        foreach (Material material in materials)
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    public void DeActivateEmission()
    {
        if (!IsHost) return;
        foreach (Material material in materials)
        {
            material.DisableKeyword("_EMISSION");
        }
    }
}
