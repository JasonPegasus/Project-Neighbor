using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class DrawerSystem : MonoBehaviour
{
    public bool open = false;
    public AudioClip s_open;
    public AudioClip s_close;
    public AudioClip s_fell;

    public void Execute(bool byneighbor)
    {
        if (!fell)
        {
            open = !open;
            GetComponent<Animator>().SetBool("Open", open);
            StartCoroutine("Change");
        }
    }

    private bool fell = false;

    IEnumerator Change()
    {
        if (open)
        {GetComponent<AudioSource>().clip = s_open;}
        else
        {GetComponent<AudioSource>().clip = s_close;}
        GetComponent<AudioSource>().Play();

        yield return new WaitForSeconds(0.25f);
        if (Random.Range(0, 15) == 0 && open)
        {  
            fell = true;
            GetComponent<Rigidbody>().useGravity = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Animator>().enabled = false;
            GetComponent<AudioSource>().clip = s_fell;
            GetComponent<AudioSource>().Play();
            Destroy(transform.Find("obstacle").gameObject);
        }
    }
}
