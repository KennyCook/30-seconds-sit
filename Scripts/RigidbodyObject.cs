using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyObject : MonoBehaviour {

    public Collider SurfaceCollider;
    private bool _isSleeping;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log(this.gameObject.GetComponent<Rigidbody>().velocity.magnitude);
    }

    private void FixedUpdate()
    {

        //if (this.gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1.0f)
        //{
        //    Debug.Log("sleep");
        //    _isSleeping = true;
        //    gameObject.GetComponent<Rigidbody>().Sleep();
        //}
        //else
        //{
        //    Debug.Log("awake"); 
        //    gameObject.GetComponent<Rigidbody>().WakeUp();
        //}

    }

    void OnCollisionEnter(Collision c)
    {
        if (this.gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1.0f)
        {
            Debug.Log("sleep");
            _isSleeping = true;
            gameObject.GetComponent<Rigidbody>().Sleep();
        }
        else
        {
            Debug.Log("awake");
            _isSleeping = false;
            gameObject.GetComponent<Rigidbody>().WakeUp();
        }
    }

    void OnCollisionExit(Collision c)
    {
        if (this.gameObject.GetComponent<Rigidbody>().velocity.magnitude <= 1.0f)
        {
            Debug.Log("sleep");
            _isSleeping = true;
            gameObject.GetComponent<Rigidbody>().Sleep();
        }
        else
        {
            Debug.Log("awake");
            _isSleeping = false;
            gameObject.GetComponent<Rigidbody>().WakeUp();
        }
    }
}
