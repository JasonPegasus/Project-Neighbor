using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSystem : MonoBehaviour
{
    public GameObject bg;

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Fix();
        }
    }

    public void Break(Vector3 direction, Vector3 impos)
    {
        GameObject bw = Instantiate(bg, transform.position, transform.rotation);
        bw.transform.localScale = transform.localScale;
        bw.transform.SetParent(gameObject.transform);
        bw.GetComponent<AfterBreak>().fc = direction;
        bw.GetComponent<AfterBreak>().ip = impos;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<AudioSource>().Play();
    }

    public void Fix()
    {
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<BoxCollider>().enabled = true;
    }
    

    // void OnCollisionEnter(Collision obj)
    // {
    //     GameObject hit = obj.gameObject;
    //     if (hit.tag == "Item")
    //     {
    //         Rigidbody rb = hit.GetComponent<Rigidbody>();
    //         if (rb.velocity.magnitude > 1)
    //         {
    //             Break(rb.velocity);
    //         }
    //     }
    // }
}