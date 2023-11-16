using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPhysics : MonoBehaviour
{
    public Vector3 currvel;
    public Vector3 currpos;
    public ItemInfo ii;
    private GameObject nei;
    private NeighborController nc;
    public GameObject lasttouch;
    public Vector3 lastpos;

    void Start()
    {
        nei = GameObject.Find("Neighbor").gameObject;
        nc = nei.GetComponent<NeighborController>();
        ii = GetComponent<ItemInfo>();
        lastpos = transform.position;
    }

    void FixedUpdate()
    {
        if (transform.tag == "Item")
        {
            currvel = GetComponent<Rigidbody>().velocity;
            currpos = transform.position;
        }
    }

    public void ChangeTouch(GameObject obj)
    {
        lasttouch = obj;
    }

    private bool break_cooldown = true;

    void OnCollisionEnter(Collision col)
    {
        GameObject obj = col.transform.gameObject;
        if (obj == GameManager.utils.player | obj.transform.tag == "Item" | obj == GameManager.utils.neighbor)
        {ChangeTouch(obj);}

        if (currvel.magnitude > 5 && obj.name != "PLAYER")
        {
            ii.audio.pitch = Random.Range(0.9f, 1.1f);
            ii.audio.Play();

            if (lasttouch == GameManager.utils.player)
            {
                nc.Hear(transform.position, 6);
            }
        }


        StartCoroutine("BreakWindow", col);
    }

    IEnumerator BreakWindow(Collision obj)
    {
        if (break_cooldown)
        {
            GetComponent<Rigidbody>().AddForce(currvel);
            GameObject hit = obj.gameObject;
            GlassSystem gs = hit.GetComponent<GlassSystem>();
            if (gs != null)
            {
                if (currvel.magnitude > 2)
                {
                    break_cooldown = false;
                    gs.Break(currvel, currpos);
                    yield return new WaitForSeconds(0.5f);
                    break_cooldown = true;
                    nc.Hear(transform.position + new Vector3((Random.Range(0,1)*2-1)*2, 0, (Random.Range(0,1)*2-1)*2), 100);
                }
            }
        }
    }
}
