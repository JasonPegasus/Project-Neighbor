using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankSystem : MonoBehaviour
{
    public int nails;
    private GameObject ItemStorage;

    void Start()
    {
        nails = this.transform.childCount-2;
        ItemStorage = GameObject.Find("_ITEMS");
    }

    public void Execute()
    {
        nails -= 1;
        if (nails < 1)
        {
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.transform.SetParent(ItemStorage.transform);
            this.gameObject.transform.tag = "Item";
            this.GetComponent<Rigidbody>().AddTorque(transform.up, ForceMode.Impulse);
            transform.Find("Sound").gameObject.GetComponent<AudioSource>().Play();
            Destroy(this);
        }
    }
}
