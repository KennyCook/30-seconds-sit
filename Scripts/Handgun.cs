using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handgun : MonoBehaviour, IChairBuilderTool
{
    public RectTransform cursor;
    public ParticleSystem muzzleFlash;
    public GameObject chairPiece;

    private int _targetsLeft = 3;

    public string GetName()
    {
        return "Pistol";
    }

    public void InitializePosition(Vector3 pos, Quaternion rot)
    {
        transform.localRotation = Quaternion.identity;
        transform.position = pos;

        transform.Translate(-0.2f, -0.15f, 0);
        transform.Rotate(90, 0, -90);
    }

    public void Use(Transform target)
    {
        try
        {
            muzzleFlash.Play();
            GetComponent<AudioSource>().Play();

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(cursor.position);

            Physics.Raycast(ray, out hit, 200f, LayerMask.GetMask("Target"));

            if (hit.transform != null)
            {
                TargetHit(hit.transform.gameObject);
            }

        }
        catch (System.Exception e) { }
    }

    private void TargetHit(GameObject target)
    {
        target.SetActive(false);

        // call method in game controller to handle number of targets hit
        _targetsLeft -= 1;
        if (_targetsLeft <= 0)
        {
            chairPiece.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}

// cooldown on fire?