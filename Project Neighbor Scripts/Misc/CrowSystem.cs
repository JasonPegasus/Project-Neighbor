using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowSystem : MonoBehaviour
{
    public GameObject loud;
    public GameObject loop;
    public GameObject crow;
    public GameObject SeeCollider;
    private bool alerted = false;
    private bool cool = false;
    
    void OnTriggerStay(Collider col)
    {
        if (col.transform.gameObject == GameManager.utils.player)
        {
            if (Physics.Raycast(GameManager.utils.cam.transform.position, GameManager.utils.cam.transform.forward, out RaycastHit hit, 200f, Physics.AllLayers))
            {
                print(hit.transform.gameObject);
                if (hit.transform.gameObject == SeeCollider | hit.transform.gameObject == gameObject)
                {StartCoroutine(StopAlert());}
                else
                {StartCoroutine(Alert());}
            }
            else
            {StartCoroutine(Alert());}
        }
    }
    
    void OnTriggerExit(Collider col)
    {
        if (col.transform.gameObject == GameManager.utils.player)
        {
            StartCoroutine(StopAlert());
        }
    }

    IEnumerator StopAlert()
    {
        alerted = false;
        StopCoroutine(Alert());
        loop.GetComponent<AudioSource>().Stop();
        crow.GetComponent<Animator>().SetBool("Moving", false);
        yield return null;
    }

    IEnumerator Alert()
    {
        StopCoroutine(StopAlert());
        if (!alerted)
        {
            alerted = true;
            cool = true;
            loud.GetComponent<AudioSource>().Play();
            crow.GetComponent<Animator>().SetBool("Moving", true);
            
            yield return new WaitForSeconds(0.5f);
            loop.GetComponent<AudioSource>().Play();
            GameManager.utils.nc.Hear(GameManager.utils.player.transform.position, 1000);
        }
    }
}
