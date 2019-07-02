using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicGlasses : MonoBehaviour, IChairBuilderTool
{
    public Material whiteMat;
    public Material chairPieceMat;
    public GameObject chairPiece;

    private bool _magicVision = false;
    private int _defaultCullingMask;

    private const int MAGIC_VISION_CULLING_MASK = -7991;

    void Start()
    {
        _magicVision = false;
        _defaultCullingMask = Camera.main.cullingMask;
    }

    public void InitializePosition(Vector3 pos, Quaternion rot)
    {
        transform.localRotation = Quaternion.identity;
        transform.position = pos;

        transform.Translate(-0.4f, -0.1f, 0);
        transform.Rotate(180, -60, -105);
    }

    public string GetName()
    {
        return "Magic Glasses";
    }

    public void Use(Transform target)
    { 
        Material mat;
        int cullingMaskVal;

        if (_magicVision)
        {
            // use ChairBrown mat
            mat = chairPieceMat;
            // revert camera culling mask settings
            cullingMaskVal = _defaultCullingMask;
        }
        else
        {
            // use shader mat
            mat = whiteMat;
            // modify camera culling mask settings
            cullingMaskVal = MAGIC_VISION_CULLING_MASK;
            // reveal invisible chair piece
            chairPiece.SetActive(true);
        }

        Camera.main.GetComponent<CameraRenderFX>().enabled = !Camera.main.GetComponent<CameraRenderFX>().enabled;
        Camera.main.cullingMask = cullingMaskVal;

        foreach (var chairPiece in GameObject.FindGameObjectsWithTag("ChairPiece"))
        {
            var mats = chairPiece.GetComponent<MeshRenderer>().materials;
            mats[0] = mat;
            chairPiece.GetComponent<MeshRenderer>().materials = mats;
        }

        _magicVision = !_magicVision;

        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }
}
