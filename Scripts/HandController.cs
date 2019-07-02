using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandController : MonoBehaviour
{
    public GameObject TargetObject;
    public Camera Eyes;
    public Text InteractableText;
    public SitOrDie.GameController gc;

    public RectTransform cursor;

    public float _useCooldown;

    private GameObject _heldItem;
    private Transform _objectHit;

    private bool _isHoldingItem;
    private bool _isUsingItem;
    private float _pickupCooldown;

    void Awake()
    {
        _pickupCooldown = 0f;
        _useCooldown = 0f;
        _isHoldingItem = false;
    }

    void Update()
    {
        _isHoldingItem = transform.childCount > 0;

        GetInput();
    }

    private void FixedUpdate()
    {
        GetOcularRaycast();
    }

    void GetInput()
    {
        // Use/Unuse held object or object being targetted
        if (Input.GetMouseButtonDown(0) && _useCooldown <= 0)
        {
            if (_isHoldingItem)
            {
                UseTool();
            }
            else if (_objectHit != null && _objectHit.gameObject.layer == LayerMask.NameToLayer("Chair") && gc.IsChairComplete())
            {
                gc.Sit();
            }
        }

        // Pick up/Put down object
        if (Input.GetMouseButtonDown(1)) //&& _pickupCooldown <= 0)
        {
            if (!_isHoldingItem)
            {
                PickupObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    void GetOcularRaycast()
    {
        RaycastHit hit;
        Ray ray = Eyes.ScreenPointToRay(cursor.position);

        if (Physics.Raycast(ray, out hit, 2f, LayerMask.GetMask("Interactable", "Chair", "Chair Piece")))
        {
            _objectHit = hit.transform;
            InteractableText.text = (_objectHit.GetComponent<IChairBuilderTool>() != null) ? _objectHit.GetComponent<IChairBuilderTool>().GetName() : "";

        }
        else
        {
            _objectHit = null;
            InteractableText.text = null;
        }
    }

    void UseTool()
    {
        TargetObject.GetComponent<IChairBuilderTool>().Use(_objectHit);
    }

    void PickupObject()
    {
        if (_objectHit != null && _objectHit.GetComponent<IChairBuilderTool>() != null)
        {
            TargetObject = _objectHit.gameObject;

            TargetObject.transform.parent = this.transform;
            TargetObject.transform.position = this.transform.position;
            SetLayer(TargetObject, "Held Item");

            // Disable physics/collider
            ToggleHeldItemCollider(TargetObject);
            TargetObject.GetComponent<Rigidbody>().isKinematic = true;

            // Initialize held item's position
            TargetObject.GetComponent<IChairBuilderTool>().InitializePosition(transform.position, transform.rotation);
        }
    }

    void DropObject()
    {
        // if glasses are in use, do not drop
        if (Camera.main.GetComponent<CameraRenderFX>().enabled)
        {
            return;
        }

        TargetObject.transform.parent = null;
        SetLayer(TargetObject, "Interactable");

        ToggleHeldItemCollider(TargetObject);
        TargetObject.GetComponent<Rigidbody>().isKinematic = false;

        _heldItem = null;
        _pickupCooldown = 0.2f;
    }

    // Set held item and its children to the layer culled by the main camera
    void SetLayer(GameObject g, string layerName)
    {
        g.layer = LayerMask.NameToLayer(layerName);

        for (int i = 0; i < g.transform.childCount; i++)
        {
            SetLayer(g.transform.GetChild(i).gameObject, layerName);
        }
    }

    void ToggleHeldItemCollider(GameObject g)
    {
        var collider = g.GetComponent<Collider>();
        if (collider != null)
        {
            collider.enabled = !collider.enabled;
        }
        for (int i = 0; i < g.transform.childCount; i++)
        {
            ToggleHeldItemCollider(g.transform.GetChild(i).gameObject);
        }
    }
}
