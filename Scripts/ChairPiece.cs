using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SitOrDie;

public class ChairPiece : MonoBehaviour, IChairBuilderTool
{
    public string PieceName;
    public Material chairBrown;

    private GameController _gc;

    private void Start()
    {
        _gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void InitializePosition(Vector3 pos, Quaternion rot)
    {
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;

        //transform.Translate(-0.2f, -0.15f, 0);
        //transform.Translate(-gameObject.GetComponent<BoxCollider>().center.x, -gameObject.GetComponent<BoxCollider>().center.y, -gameObject.GetComponent<BoxCollider>().center.z);

        switch (transform.gameObject.name)
        {
            case "Chair Seat":
                transform.Translate(-0.2f, -0.2f, 0.1f);
                transform.Rotate(0, -90, 20);
                break;
            case "Chair Leg FR":
                transform.Translate(-0.3f, -0.3f, 0.1f);
                transform.Rotate(120, 0, 0);
                break;
            case "Chair Leg BR":
                transform.Translate(-0.6f, -0.3f, 0.1f);
                transform.Rotate(120, 0, 0);
                break;
            case "Chair Leg FL":
                transform.Translate(-0.3f, -0.2f, -0.1f);
                transform.Rotate(120, 0, 0);
                break;
            case "Chair Leg BL":
                transform.Translate(-0.6f, -0.2f, -0.1f);
                transform.Rotate(120, 0, 0);
                break;
            default:
                break;
        }
    }

    public string GetName()
    {
        return "Chair Piece";
    }

    public void Use(Transform target)
    {
        if (target == null) { return; }

        // if object player is targetting chair blueprint then
        if (target.name == "Chair")
        {
            //      find child of blueprint with matching PieceName
            var childPiece = target.Find(PieceName);

            //      set child's material to chairBrown
            var mats = childPiece.GetComponent<MeshRenderer>().materials;
            mats[0] = chairBrown;
            childPiece.GetComponent<MeshRenderer>().materials = mats;

            //      delete(?) this gameobject
            Destroy(gameObject);

            _gc.AddChairPiece();
        }
    }
}
