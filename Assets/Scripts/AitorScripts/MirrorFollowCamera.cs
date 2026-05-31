using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class MirrorFollowCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraM;
    [SerializeField] GameObject mirror;

    // Update is called once per frame
    void Update()
    {
        //calculate the position and the rotation of the player to change what he sees in the mirror
        this.transform.position = new Vector3(cameraM.transform.position.x, 
                                  this.transform.position.y, this.transform.position.z);

        Vector3 dirToMirror = (mirror.transform.position - cameraM.transform.position).normalized;

        //calculate rotation
        float angleRad = Mathf.Atan2(dirToMirror.x, dirToMirror.z);
        float angleDeg = angleRad * Mathf.Rad2Deg;
        Debug.Log(angleDeg);
        this.transform.rotation = Quaternion.Euler(this.transform.eulerAngles.x, 
                                  180-angleDeg, this.transform.eulerAngles.z);
    }
}
