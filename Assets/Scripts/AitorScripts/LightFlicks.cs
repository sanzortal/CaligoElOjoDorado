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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flickedLight = this.gameObject.GetComponent<Light>();
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

                yield return new WaitForSeconds(flickShutDownDuration);
                flickedLight.enabled = true;
                flicksDone++;

                yield return new WaitForSeconds(timebetweenFlicks);
            }

            flickedLight.enabled = false;
            yield return new WaitForSeconds(lastFlickDuration);
            flickedLight.enabled = true;
            yield return new WaitForSeconds(4f);
        }
    }
}
