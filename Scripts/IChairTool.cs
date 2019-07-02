using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChairBuilderTool{
    void Use(Transform t);
    void InitializePosition(Vector3 pos, Quaternion rot);
    string GetName();
    //bool GetInteractable();
}
