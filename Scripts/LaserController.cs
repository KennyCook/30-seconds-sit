using SitOrDie;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public GameController GC;
    public AudioSource laserCollide;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            this.GetComponent<BoxCollider>().enabled = false;
            laserCollide.Play();
            GC.OnLaserCollide();
        }
    }
}