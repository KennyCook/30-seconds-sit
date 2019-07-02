using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRenderFX : MonoBehaviour 
{
    public Material RedMat;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, RedMat);
    }

}
