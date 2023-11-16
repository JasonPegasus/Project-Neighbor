using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightSystem : MonoBehaviour
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
    
    void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        light = transform.Find("Light").gameObject;
        s2 = GetComponent<AudioSource>();
    }

    void Update()
    {
        if(transform.gameObject.activeSelf == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChangeState();
            }
        }
    }

    private void ChangeState()
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
    }

    public void TurnOn()
    {
        turned = true;
        mr.material = m_on;
        light.SetActive(true);
    }

    public void TurnOff()
    {
        turned = false;
        mr.material = m_off;
        light.SetActive(false);
    }
}
