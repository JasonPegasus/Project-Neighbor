using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterBreak : MonoBehaviour
{
    public GameObject[] shatters;
    public Vector3 fc;
    public Vector3 ip;

    void Start()
    {
        foreach (Transform shatters in transform)
        {
            GameObject sh = shatters.gameObject;
            sh.AddComponent<Rigidbody>();
            sh.AddComponent<BoxCollider>();
            sh.GetComponent<BoxCollider>().size += new Vector3(0, 0, 0.01f);
            sh.GetComponent<Renderer>().material = GetComponent<Renderer>().material;
            sh.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            if (fc != null)
            {
                float dist = Vector3.Distance(ip, sh.transform.position);
                //sh.GetComponent<Rigidbody>().mass = 1f;
                sh.GetComponent<Rigidbody>().AddForce(fc/(dist/10));
            }
            Destroy(sh.transform.parent.gameObject, 2);
        }
    }
}
