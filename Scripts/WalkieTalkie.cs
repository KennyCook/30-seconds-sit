using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkieTalkie : MonoBehaviour, IChairBuilderTool
{
	public AudioSource Playback;
    public GameObject MusicDoor;

    public void InitializePosition(Vector3 pos, Quaternion rot)
    {
        transform.localRotation = Quaternion.identity;
        transform.position = pos;

        transform.Translate(-0.15f, 0.18f, 0.1f);
        transform.Rotate(-75, -20, 0);
    }

    public string GetName()
    {
        return "Radio";
    }

    public void Use(Transform target)
	{
		if (!Playback.isPlaying)
		{
			Playback.Play();
            StartCoroutine(OpenMusicDoor());

		}
		else
		{
			Playback.Stop();
		}
	}

    IEnumerator OpenMusicDoor()
    {
        //print(Time.time);
        //yield return new WaitForSeconds(5);
        //print(Time.time);
        yield return new WaitForSeconds(12.5f);
        MusicDoor.GetComponent<Animator>().Play("anim_MusicDoor");
        //MusicDoor.gameObject.SetActive(false);
    }
}
