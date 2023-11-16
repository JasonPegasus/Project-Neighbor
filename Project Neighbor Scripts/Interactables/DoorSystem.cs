using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DoorSystem : MonoBehaviour
{
    private GameObject door;
    private NavMeshObstacle obs;

    public bool isopen = false;
    public bool Locked;
    public float AngMin;
    public float AngMax;

    public AudioClip s_open;
    public AudioClip s_close;
    public AudioClip s_locked;
    private new AudioSource audio;
    private BoxCollider col;
    private BoxCollider col2;
    private NeighborController nc;

    void Start()
    {
        door = gameObject;
        obs = GetComponent<NavMeshObstacle>();
    }
    

    public void Execute(GameObject thedoor, float speed, float soundlv, bool closeafter = false)
    {
        door = thedoor;
        audio = GetComponent<AudioSource>();
        col = GetComponent<BoxCollider>();
        col2 = transform.Find("Collider").GetComponent<BoxCollider>();
        nc = GameObject.Find("Neighbor").gameObject.GetComponent<NeighborController>();
        if (isUnlocked(transform.parent.transform.parent.gameObject))
        {
            if (!isopen)
            {StartCoroutine("Open");}
            else
            {StartCoroutine("Close");}


            if (closeafter)
            {StartCoroutine("WaitClose");}
            else
            {nc.Hear(transform.position + new Vector3((Random.Range(0,1)*2-1)*2, 0, (Random.Range(0,1)*2-1)*2), 6);}
        }
        else
        {
            audio.clip = s_locked;
            audio.Play();
        }
    }

    public bool isUnlocked(GameObject thedoor)
    {
        if (!Locked)
        {
            if (thedoor.transform.Find("Lock") != null | thedoor.transform.Find("Chair") != null | thedoor.transform.Find("Plank") != null)
            {
                obs.enabled = true;
                return false;
            }
            else
            {
                Locked = false;
                obs.enabled = false;
                return true;
            }
        }
        else
        {
            obs.enabled = true;
            return false;
        }
    }

    IEnumerator UpdateLock()
    {
        while (true)
        { 
            yield return new WaitForSeconds(1);
            isUnlocked(door);
        }
    }

    IEnumerator WaitClose()
    {
        yield return new WaitForSeconds(0.8f);
        StartCoroutine("Close");
    }

    IEnumerator Close()
    {
        isopen = false;
        col.enabled = false;
        col2.enabled = false;
        GetComponent<Animator>().SetBool("Open", false);
        yield return new WaitForSeconds(0.4f);
        audio.clip = s_close;
        audio.Play();
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
        col2.enabled = true;
    }
    
    IEnumerator Open()
    {
        isopen = true;
        col.enabled = false;
        col2.enabled = false;
        yield return null;
        GetComponent<Animator>().SetBool("Open", true);
        audio.clip = s_open;
        audio.Play();
        yield return new WaitForSeconds(0.5f);
        col.enabled = true;
        col2.enabled = true;
    }

    

    // IEnumerator Open(float sp, float sd)
    // {
    //     door.GetComponent<BoxCollider>().enabled = true;
    //     door.transform.Find("Collider").GetComponent<BoxCollider>().enabled = true;
    //     while (door.transform.localRotation.eulerAngles.y < AngMax && isopen)
    //     { 
    //         yield return null;
    //         door.transform.RotateAround(hinge.position, Vector3.up, sp*4f);
    //     }
    // }
}
