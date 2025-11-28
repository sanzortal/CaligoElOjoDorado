using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDeaths : MonoBehaviour
{
    [SerializeField] DeathsController dc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rigid body cuando funcione
    }
    public IEnumerator die(string enemyKiller)
    {
        //animacion de morir con animation.play(enemykiller) al tener diferentes animaciones

        //turn off camera
        Transform respawn = dc.ActivatePanel();
        //wait
        yield return new WaitForSeconds(3);

        //respawn player
        this.transform.position = respawn.position;
        this.transform.eulerAngles = respawn.eulerAngles;

        //turn on camera
        dc.DeactivatePanel();

    }
}
