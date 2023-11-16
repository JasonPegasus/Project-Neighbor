using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVSystem : MonoBehaviour
{

    public bool turned = false;
    public Material m_on;
    public Material m_off;
    private MeshRenderer mr;
    private new GameObject light;
    private AudioSource s2;
    public AudioClip s_on;
    public AudioClip s_off;
    private ItemInfo ii;
    public void Execute(bool byneighbor)
    {
        Declare();
        ChangeState(byneighbor);
    }

    private void Declare()
    {
        mr = transform.Find("Model").gameObject.GetComponent<MeshRenderer>();
        light = transform.Find("Light").gameObject;
        s2 = transform.Find("Model").gameObject.GetComponent<AudioSource>();
        ii = GetComponent<ItemInfo>();
    }

    private void ChangeState(bool byneighbor)
    {
        if (turned)
        {
            TurnOff();
            s2.clip = s_off;
        }
        else
        {
            TurnOn();
            s2.clip = s_on;
        }
        s2.Play();
        
        if (!byneighbor)
        {
            GameManager.utils.nc.Hear(transform.position, 20);
        }
    }

    public void TurnOn()
    {
        turned = true;
        mr.material = m_on;
        light.SetActive(true);
        StartCoroutine("Effects");
        GetComponent<AudioSource>().volume = 0.8f;
        if (!GetComponent<AudioSource>().isPlaying)
        {GetComponent<AudioSource>().Play();}
    }

    public void TurnOff()
    {
        Declare();
        turned = false;
        mr.material = m_off;
        light.SetActive(false);
        StopCoroutine("Effects");
        GetComponent<AudioSource>().volume = 0f;
    }

    IEnumerator Effects()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0f, 1.1f));
            float ran = Random.Range(10 , 100);
            light.GetComponent<Light>().intensity = ran;
            mr.material.SetColor("_EmissiveColor", Color.white * ran/20);
        }
    }
}
