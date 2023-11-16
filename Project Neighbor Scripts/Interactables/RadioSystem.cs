using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioSystem : MonoBehaviour
{

    public bool turned = false;
    public Material m_on;
    public Material m_off;
    private MeshRenderer mr;
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
        GetComponent<AudioSource>().volume = 0.8f;
        if (!GetComponent<AudioSource>().isPlaying)
        {GetComponent<AudioSource>().Play();}
    }

    public void TurnOff()
    {
        Declare();
        turned = false;
        mr.material = m_off;
        GetComponent<AudioSource>().volume = 0f;
    }
}
