using System.Collections;
using UnityEngine;

public class LightFlicks : MonoBehaviour
{
    Light flickedLight;
    [SerializeField] int flickCount;
    [SerializeField] float timebetweenFlicks;
    [SerializeField] float flickShutDownDuration;
    [SerializeField] bool flick;
    [SerializeField] float lastFlickDuration;
    [SerializeField] Color offColorLight;
    private Color onColorLight;
    private Material lightMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flickedLight = this.gameObject.GetComponent<Light>();

        foreach (Material mat in GetComponentInParent<MeshRenderer>().materials)
        {
            if (mat.name.Equals("yellowOnLight (Instance)"))
            {
                lightMaterial = mat;
                onColorLight = lightMaterial.color;
            }
        }

        StartCoroutine(Flicks());
    }

    IEnumerator Flicks()
    {
        int flicksDone;

        while (flick)
        {
            flicksDone = 0;
            while (flicksDone < flickCount)
            {
                flickedLight.enabled = false;
                lightMaterial.color = offColorLight;

                yield return new WaitForSeconds(flickShutDownDuration);
                flickedLight.enabled = true;
                lightMaterial.color = onColorLight;

                flicksDone++;

                yield return new WaitForSeconds(timebetweenFlicks);
            }

            flickedLight.enabled = false;
            lightMaterial.color = offColorLight;

            yield return new WaitForSeconds(lastFlickDuration);
            flickedLight.enabled = true;
            lightMaterial.color = onColorLight;

            yield return new WaitForSeconds(4f);
        }
    }
}
