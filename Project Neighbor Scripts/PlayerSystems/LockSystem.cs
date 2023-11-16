using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockSystem : MonoBehaviour
{
    public string LockType;
    public string LockName;
    public bool Locked = true;
    public GameObject ChangedObject;
    private GameObject ItemStorage;

    public void Execute()
    {
        ItemStorage = GameObject.Find("_ITEMS");
        if (Locked)
        {
            if (LockType == "key")
            {
                unlockKEY();
            }
            if (LockType == "nail")
            {
                unlockPLANK();
            }
        }
    }

    void unlockKEY()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
        Locked = false;
        if (ChangedObject != null)
        {
            ChangedObject.GetComponent<DoorSystem>().Locked = false;
        }
        this.gameObject.transform.SetParent(ItemStorage.transform);
        this.gameObject.transform.tag = "Item";
    }

    void unlockPLANK()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
        this.GetComponent<Rigidbody>().AddForce(-transform.up/10 - transform.forward/6, ForceMode.Impulse);
        this.GetComponent<Rigidbody>().AddTorque(transform.up*100, ForceMode.Impulse);
        float rand = 10f;
        this.transform.Translate(transform.forward * 0.05f);
        this.transform.Rotate(Random.Range(-rand, rand), Random.Range(-rand, rand), Random.Range(-rand, rand));
        this.gameObject.transform.SetParent(ItemStorage.transform);
        this.gameObject.transform.tag = "Item";
        Locked = false;
        transform.Find("Sound").gameObject.GetComponent<AudioSource>().Play();
        if (ChangedObject != null)
        {
            ChangedObject.GetComponent<PlankSystem>().Execute();
        }
    }
}
