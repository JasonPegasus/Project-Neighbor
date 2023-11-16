using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedSystem : MonoBehaviour
{
    public bool used = false;
    public AudioClip s_enter;
    public AudioClip s_exit;
    private GameObject plr;
    private Rigidbody rb;
    private Vector3 point;
    private Vector3 lastpos = Vector3.zero;

    public void Execute()
    {
        used = !used;

        plr = GameManager.utils.player;
        point = transform.Find("Point").transform.position;
        rb = plr.GetComponent<Rigidbody>();

        if (used)
        {StartCoroutine("GetIn");}
        else
        {StartCoroutine("GetOut");}
    }

    IEnumerator GetIn()
    {
        StopCoroutine("GetOut");
        GetComponent<AudioSource>().clip = s_enter;
        GetComponent<AudioSource>().Play();
        lastpos = plr.transform.position + new Vector3(0,0.5f,0);
        while (true)
        {
            yield return null;
            plr.transform.position = Vector3.Lerp(plr.transform.position, point, 0.1f);
            rb.isKinematic = true;
            rb.useGravity = false;
            plr.GetComponent<CharacterController>().enabled = false;
        }
    }
    

    IEnumerator GetOut()
    {
        StopCoroutine("GetIn");
        GetComponent<AudioSource>().clip = s_exit;
        GetComponent<AudioSource>().Play();
        int i = 0;
        while (i<20)
        {
            i++;
            yield return null;
            plr.transform.position = Vector3.Lerp(plr.transform.position, lastpos, 0.1f);
        }
        rb.isKinematic = false;
        rb.useGravity = true;
        plr.GetComponent<CharacterController>().enabled = true;
    }
}
