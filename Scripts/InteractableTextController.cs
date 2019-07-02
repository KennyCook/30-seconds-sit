using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTextController : MonoBehaviour
{
    private void Update()
    {
        //void OnGUI()
        //{
        //    float xMin = (Screen.width / 2) - (crosshairImage.width / 2);
        //    float yMin = (Screen.height / 2) - (crosshairImage.height / 2);

        //    print(System.String.Format("{0}, {1}", xMin, yMin));

        //    // Crosshair
        //    GUI.DrawTexture(new Rect(xMin, yMin, (crosshairImage.width * 0.5f), (crosshairImage.height * 0.5f)), crosshairImage);

        //    // Interactable Name
        //    GUIContent gc = new GUIContent((_objectHit == null ? "" : _objectHit.name));
        //    GUIStyle gs = new GUIStyle()
        //    {
        //        alignment = TextAnchor.UpperCenter
        //    };
        //    //GUI.Label(new Rect(xMin, yMin, 100f, 100f), (_objectHit == null ? "" : _objectHit.name), new GUIStyle() { alignment = TextAnchor.MiddleCenter } );
        //    GUI.Label(new Rect(xMin, yMin, gs.CalcSize(gc).x, gs.CalcSize(gc).y), gc, gs);
        //}
    }
}
