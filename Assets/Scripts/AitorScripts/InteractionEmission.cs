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

    //get all the emission materials
    public void SetMaterials()
    {
        materials = this.gameObject.GetComponent<Renderer>().materials;
    }

    //activate the emission materials
    public void ActivateEmission()
    {
        if (!IsHost) return;
        foreach (Material material in materials)
        {
            material.EnableKeyword("_EMISSION");
        }
    }

    //deactivate the emission materials
    public void DeActivateEmission()
    {
        
        foreach (Material material in materials)
        {
            material.DisableKeyword("_EMISSION");
        }
    }
}
