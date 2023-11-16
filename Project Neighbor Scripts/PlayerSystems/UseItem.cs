using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public string UseType;
    public string Unlocks;
    public bool DestroyAfterUse;
    public float maxdist = 2.5f;
    private InventorySystem IS;

    private GameObject cam;
    private GameObject plr;

    void Start()
    {
        IS = GameObject.Find("_INVENTORY").GetComponent<InventorySystem>();
        cam = IS.Player.transform.Find("Camera").gameObject;
        plr = IS.Player;
    }
    

    void Update()
    {
        OnClick();
    }

    void OnClick()
    {
        if (this.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (UseType == "key")
                {
                    KeyUnlock();
                }
                else if(UseType == "nail")
                {
                    NailUnlock();
                }
                else if(UseType == "chair")
                {
                    ChairLock();
                }
            }
        }
    }

    void ChairLock()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxdist, Physics.AllLayers))
        {
            GameObject ho = hit.transform.gameObject;
            if (ho.GetComponent<DoorSystem>() != null)
            {
                GameObject chair = GameManager.utils.inv.ThrowItem(false);
                chair.transform.position = ho.transform.parent.transform.parent.Find("ChairPosition").position;
                chair.transform.rotation = ho.transform.parent.transform.parent.Find("ChairPosition").rotation;
                chair.GetComponent<Rigidbody>().isKinematic = true;
                chair.GetComponent<Rigidbody>().useGravity = false;
                chair.transform.SetParent(ho.transform.parent.transform.parent);
            }
        }
    }

    void KeyUnlock()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxdist, Physics.AllLayers))
        {
            GameObject ho = hit.transform.gameObject;
            if (hit.transform.tag == "Unlockable" && ho.GetComponent<LockSystem>().LockType == UseType)
            {
                if (ho.GetComponent<LockSystem>().LockName == Unlocks | Unlocks == "all")
                {
                    ho.GetComponent<LockSystem>().Execute();
                }
            }
        }
    }

    void NailUnlock()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxdist, Physics.AllLayers))
        {
            GameObject ho = hit.transform.gameObject;
            if (hit.transform.tag == "Unlockable" && ho.GetComponent<LockSystem>().LockType == UseType)
            {
                ho.GetComponent<LockSystem>().Execute();
            }
        }
    }
}
