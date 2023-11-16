using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    [Header("Basic")]
    public GameObject Player;
    private GameObject ItemStorage;
    public GameObject ITEMS;
    public Sprite Invisible;
    public LayerMask blocklayer;


    void Start()
    {
        ItemStorage = GameObject.Find("_ITEMS");
        slotequipped = s1;
        s1.transform.localScale = minSize;
        s2.transform.localScale = minSize;
        s3.transform.localScale = minSize;
        s4.transform.localScale = minSize;

        cam = Player.transform.Find("Camera");
    }

    void Update()
    {
        Selection();
        GrabActive();
        ItemView();
        DetectThrow();
        //MenuHide();
    }
    
    private bool hiden;

    void MenuHide()
    {
        
    }

    private Transform cam;
    [Header("Grab Options")]
    public float MAX_GRAB_DISTANCE;

    void GrabActive()
    {
        float maxgrab = MAX_GRAB_DISTANCE;
        Debug.DrawRay(cam.position, cam.forward * maxgrab, Color.green);

        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Physics.Raycast(cam.position, cam.forward, out hit, maxgrab, blocklayer))
            {
                if (hit.transform.tag == "Item")
                {
                    if (hasEmpty())
                    {
                        if (GetFirstEmpty() != null)
                        {
                            ResetState(hit.transform.gameObject);
                            slotequipped = GetFirstEmpty();
                            ItemInfo slotinf = GetFirstEmpty().GetComponent<ItemInfo>();
                            ItemInfo iteminf = hit.transform.gameObject.GetComponent<ItemInfo>();
                            slotinf.ItemName = iteminf.ItemName;
                            // slotinf.Type = iteminf.Type;
                            slotinf.ItemObject = iteminf.ItemObject;
                            slotequipped.transform.Find("ItemImage").GetComponent<Image>().sprite = iteminf.Icon;
                            GrabItem(hit.transform.gameObject);
                            RemoveLastVisual();
                        }
                    }
                }
            }
        }
    }

    void GrabItem(GameObject item)
    {
        item.transform.SetParent(ItemStorage.transform);
        item.transform.position = ItemStorage.transform.position;
        Rigidbody itembody = item.GetComponent<Rigidbody>();
        itembody.velocity = Vector3.zero;
        itembody.angularVelocity = Vector3.zero;
        itembody.isKinematic = false;
        itembody.useGravity = true;
        item.SetActive(false);
    }

    void ResetState(GameObject obj)
    {
        InteractableInfo ii = obj.GetComponent<InteractableInfo>();
        if (ii != null)
        {
            if (ii.Type == "tv")
            {
                obj.GetComponent<TVSystem>().TurnOff();
            }
            if (ii.Type == "radio")
            {
                obj.GetComponent<RadioSystem>().TurnOff();
            }
            if (ii.Type == "flashlight")
            {
                GameObject fl = cam.transform.Find("ItemVisual").Find("flashlight").gameObject;
                fl.SetActive(true);
                fl.GetComponent<FlashlightSystem>().TurnOff();
                fl.SetActive(false);
            }
        }
    }
    
    public GameObject itemvisual;
    public GameObject lastvisual;

    void ItemView()
    {
        if (slotChanged() == true)
        {
            RemoveLastVisual();
        }
        if (!isEmpty(slotequipped))
        {
            string itemname = slotequipped.GetComponent<ItemInfo>().ItemName;
            GameObject itemtoview = itemvisual.transform.Find(itemname).gameObject;
            itemtoview.SetActive(true);
            lastvisual = itemtoview;
        }
        else
        {
        }
    }

    void RemoveLastVisual()
    {
        if (lastvisual != null)
        {
            lastvisual.SetActive(false);
        }
    }

    // THROW ITEM //

    private float presstime = 0;
    public float THROW_HOLD;
    public float THROW_POWER;

    void DetectThrow()
    {
        if (!isEmpty(slotequipped))
        {
            if (Input.GetMouseButtonDown(1))
            {
                presstime = Time.time;
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (presstime - Time.time < -THROW_HOLD)
                {
                    ThrowItem(true);
                }
                else
                {
                    ThrowItem(false);
                }
            }
        }
    }

    public GameObject ThrowItem(bool ranged)
    {
        GameObject ItemStored = GetItemObject(slotequipped);
        if (ranged)
        {
            SpawnFromStorage(ItemStored, cam.transform.position + cam.transform.forward, Player.transform.rotation, cam.forward * THROW_POWER  + cam.up * (THROW_POWER/5), 20);
        }
        else
        {
            SpawnFromStorage(ItemStored, cam.transform.position + cam.transform.forward, Player.transform.rotation, new Vector3(0,0,0), 0);
        }
        ItemStored.GetComponent<ItemPhysics>().ChangeTouch(Player);
        ClearSlot();
        return ItemStored;
    }

    public void ClearSlot()
    {
        SetItemName(slotequipped, "");
        // SetItemType(slotequipped, "");
        SetItemSprite(slotequipped, Invisible);
        ClearItemObject(slotequipped);
        RemoveLastVisual();
    }

    void SpawnFromStorage(GameObject item, Vector3 pos, Quaternion rot, Vector3 force, float torque)
    {
        item.transform.SetParent(ITEMS.transform);
        item.SetActive(true);
        item.transform.position = pos;
        item.transform.rotation = rot;
        Rigidbody itembody = item.GetComponent<Rigidbody>();
        itembody.AddForce(force);
        itembody.AddTorque(new Vector3(Random.Range(-torque, torque)/10, Random.Range(-torque, torque)/10,Random.Range(-torque, torque)/10));
    }





















    [Header("Visual")]
    public GameObject s1;
    public GameObject s2;
    public GameObject s3;
    public GameObject s4;
    public GameObject bg;
    public float lerpspeed;

    private Vector3 minSize = new Vector3(0.9f, 1.4f, 0.9f);
    private Vector3 maxSize = new Vector3(1.15f, 1.65f, 1.15f);

    void Selection()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            slotequipped = s1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            slotequipped = s2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            slotequipped = s3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            slotequipped = s4;
        }
        // ANIMATION //
        if (isEquipped(s1))
        {
            s1.transform.localScale = new Vector3(Mathf.Clamp(s1.transform.localScale.x + lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s1.transform.localScale.y + lerpspeed, minSize.y, maxSize.y), 1);
        }
        else
        {
            s1.transform.localScale = new Vector3(Mathf.Clamp(s1.transform.localScale.x - lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s1.transform.localScale.y - lerpspeed, minSize.y, maxSize.y), 1);
        }
        if (isEquipped(s2))
        {
            s2.transform.localScale = new Vector3(Mathf.Clamp(s2.transform.localScale.x + lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s2.transform.localScale.y + lerpspeed, minSize.y, maxSize.y), 1);
        }
        else
        {
            s2.transform.localScale = new Vector3(Mathf.Clamp(s2.transform.localScale.x - lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s2.transform.localScale.y - lerpspeed, minSize.y, maxSize.y), 1);
        }
        if (isEquipped(s3))
        {
            s3.transform.localScale = new Vector3(Mathf.Clamp(s3.transform.localScale.x + lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s3.transform.localScale.y + lerpspeed, minSize.y, maxSize.y), 1);
        }
        else
        {
            s3.transform.localScale = new Vector3(Mathf.Clamp(s3.transform.localScale.x - lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s3.transform.localScale.y - lerpspeed, minSize.y, maxSize.y), 1);
        }
        if (isEquipped(s4))
        {
            s4.transform.localScale = new Vector3(Mathf.Clamp(s4.transform.localScale.x + lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s4.transform.localScale.y + lerpspeed, minSize.y, maxSize.y), 1);
        }
        else
        {
            s4.transform.localScale = new Vector3(Mathf.Clamp(s4.transform.localScale.x - lerpspeed, minSize.x, maxSize.x), Mathf.Clamp(s4.transform.localScale.y - lerpspeed, minSize.y, maxSize.y), 1);
        }
    }
    
    private GameObject slotequipped;

    bool isEquipped(GameObject sloteq)
    {
        if (slotequipped == sloteq)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // HELPERS //

    string GetItemName(GameObject sloteq)
    {
        ItemInfo itf = sloteq.GetComponent<ItemInfo>();
        return itf.ItemName;
    }
    
    // string GetItemType(GameObject sloteq)
    // {
    //     ItemInfo itf = sloteq.GetComponent<ItemInfo>();
    //     return itf.Type;
    // }

    GameObject GetItemObject(GameObject sloteq)
    {
        ItemInfo itf = sloteq.GetComponent<ItemInfo>();
        return itf.ItemObject;
    }

    void SetItemName(GameObject sloteq, string set)
    {
        ItemInfo itf = sloteq.GetComponent<ItemInfo>();
        itf.ItemName = set;
    }
    
    
    // void SetItemType(GameObject sloteq, string set)
    // {
    //     ItemInfo itf = sloteq.GetComponent<ItemInfo>();
    //     itf.Type = set;
    // }

    
    void SetItemSprite(GameObject sloteq, Sprite set)
    {
        slotequipped.transform.Find("ItemImage").GetComponent<Image>().sprite = set;
    }

    
    void ClearItemObject(GameObject sloteq)
    {
        ItemInfo itf = sloteq.GetComponent<ItemInfo>();
        itf.ItemObject = null;
    }

    bool hasEmpty()
    {
        if (GetItemName(s1) == "" | GetItemName(s2) == "" | GetItemName(s3) == "" | GetItemName(s4) == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool isEmpty(GameObject sloteq)
    {
        if (GetItemName(sloteq) == "")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool slotChanged()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) | Input.GetKeyDown(KeyCode.Alpha2) | Input.GetKeyDown(KeyCode.Alpha3) | Input.GetKeyDown(KeyCode.Alpha4))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    GameObject GetFirstEmpty()
    {
        if (hasEmpty())
        {
            if (GetItemName(s1) == "")
            {
                return s1;
            }
            else
            {
                if (GetItemName(s2) == "")
                {
                    return s2;
                }
                else
                {
                    if (GetItemName(s3) == "")
                    {
                        return s3;
                    }
                    else
                    {  
                        if (GetItemName(s4) == "")
                        {
                            return s4;
                        }
                        else
                        {
                            return null;
                        }
                    }
                }
            }
        }
        return null;
    }
}
