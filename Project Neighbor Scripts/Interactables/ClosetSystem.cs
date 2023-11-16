using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ClosetSystem : MonoBehaviour
{
    public bool open = false;
    public AudioClip s_open;
    public AudioClip s_close;

    IEnumerator Change()
    {
        yield return null;
        open = !open;
        GameObject hide = transform.Find("HideBox").gameObject;
    
        for(int i = 0; i <= 1; i++)
        {
            GameObject door = gameObject.transform.GetChild(i).Find("Door").gameObject;
            if (open)
            {
                door.GetComponent<AudioSource>().clip = s_open;
                hide.transform.localPosition = new Vector3(0,0, -0.01313f);
            }
            else
            {
                door.GetComponent<AudioSource>().clip = s_close;
                hide.transform.localPosition = new Vector3(0,0, 0.01313f);
            }
            door.GetComponent<AudioSource>().pitch = Random.Range(0.7f, 1.3f);
            door.GetComponent<AudioSource>().Play();
            door.GetComponent<Animator>().SetBool("Open", open);
        }
    }
}
