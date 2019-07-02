using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammer : MonoBehaviour, IChairBuilderTool
{
    public Animator hammerAnim;
    public RectTransform cursor;
    public AudioClip goodHit, badHit;

    private bool _beginUpswingClicked, _beginDownswingClicked;
    private int _wallHP = 100;

    private void Start()
    {
        _beginUpswingClicked = _beginDownswingClicked = false;
    }

    private void Update()
    {
        if (_beginUpswingClicked && _beginDownswingClicked && hammerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "anim_HammerDownswing")
        {
            _beginDownswingClicked = _beginUpswingClicked = false;
        }
    }

    public void InitializePosition(Vector3 pos, Quaternion rot)
    {
        transform.localRotation = Quaternion.identity;
        transform.position = pos;

        transform.Translate(-0.4f, 0.1f, 0.1f);
        transform.Rotate(20, 0, 90);

        _beginUpswingClicked = _beginDownswingClicked = false;
        hammerAnim.Play("Rest");
    }

    public string GetName()
    {
        return "Sledgehammer";
    }

    public void Use(Transform target)
    {
        if (!_beginUpswingClicked)
        {
            _beginUpswingClicked = true;
            hammerAnim.Play("anim_HammerUpswing");
        }
        else if (_beginUpswingClicked && !_beginDownswingClicked)
        {
            //try { print(hammerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime); } catch (System.Exception e) { } // GetCurrentAnimatorStateInfo(0)

            _beginDownswingClicked = true;
            hammerAnim.Play("anim_HammerDownswing");

            // check for cracked wall hit
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(cursor.position);
            Physics.Raycast(ray, out hit, 2f, LayerMask.GetMask("Cracked Wall"));

            if (hit.collider != null)
            {
                HammerHit(hit.collider.gameObject, hammerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
            }
        }
        else
            return;

        //if (!_beginUpswingClicked)
        //{
        //    // begin upswing
        //    _beginUpswingClicked = true;
        //    hammerAnim.SetBool("_beginUpswingClicked", _beginUpswingClicked);
        //}
        //else if (_beginUpswingClicked && !_beginDownswingClicked)
        //{
        //    // begin downswing
        //    _beginDownswingClicked = true;
        //    hammerAnim.SetBool("_beginDownswingClicked", _beginDownswingClicked);
        //}
        //else
        //    return;
    }

    private void HammerHit(GameObject crackedWall, float chargeTime)
    {
        // if hammer swing is > ~65% reduce HP by 50, play good hit sound
        if (chargeTime >= 0.65f)
        {
            GetComponent<AudioSource>().clip = goodHit;
            GetComponent<AudioSource>().Play();
            _wallHP -= 50;
        }
        // else reduce HP by 10, play bad hit sound
        else
        {
            GetComponent<AudioSource>().clip = badHit;
            GetComponent<AudioSource>().Play();
            _wallHP -= 10;
        }

        if (_wallHP <= 0)
        {
            crackedWall.SetActive(false);
        }
    }
}
