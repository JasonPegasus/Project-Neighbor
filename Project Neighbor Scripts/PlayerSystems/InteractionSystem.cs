using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSystem : MonoBehaviour
{
    private GameObject point;
    private GameObject inv;
    public GameObject cam;
    public LayerMask blocklayer;

    private float pointMin = 4f;
    private float pointMax = 10f;
    private float pointAdd = 0.5f;


    public float MAX_INTERACTION_DISTANCE;


    void Start()
    {
        inv = GameObject.Find("_INVENTORY").gameObject;
        point = inv.transform.Find("Point").gameObject;
    }

    void Update()
    {
        DetectInteract();
    }
    

    void DetectInteract()
    {
        float maxint = MAX_INTERACTION_DISTANCE;
        Debug.DrawRay(cam.transform.position, cam.transform.forward * maxint, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, maxint, blocklayer))
        {
            if (hit.transform.tag == "Interactable" | hit.transform.tag == "Item" | hit.transform.tag == "Unlockable" | hit.transform.tag == "FakeInteractuable")
            {
                PointSizes(true);
                if (hit.transform.tag == "Interactable" | hit.transform.tag == "Item")
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        InteractionSelector(hit.transform.gameObject);
                    }
                }
            }
            else
            {
                PointSizes(false);
            }
        }
        else
        {
            PointSizes(false);
        }
    }

    void InteractionSelector(GameObject obj)
    {
        InteractableInfo ii = obj.GetComponent<InteractableInfo>();
        if (ii != null)
        {
            if (ii.Type == "door")
            {
                obj.GetComponent<DoorSystem>().Execute(obj, ii.Speed, ii.SoundLevel);
            }
            if (ii.Type == "tv")
            {
                obj.GetComponent<TVSystem>().Execute(false);
            }
            if (ii.Type == "radio")
            {
                obj.GetComponent<RadioSystem>().Execute(false);
            }
            if (ii.Type == "drawer")
            {
                obj.GetComponent<DrawerSystem>().Execute(false);
            }
            if (ii.Type == "closet")
            {
                obj.GetComponent<ClosetDoor>().Execute(false);
            }
            if (ii.Type == "bed")
            {
                obj.GetComponent<BedSystem>().Execute();
            }
        }
    }

    void PointSizes(bool act)
    {
        RectTransform pointRT = point.GetComponent<RectTransform>();
        if (act)
        {
            float adder = Mathf.Clamp(pointRT.sizeDelta.x + pointAdd, pointMin, pointMax);
            pointRT.sizeDelta = new Vector2(adder, pointRT.sizeDelta.x);
        }
        else
        {
            float adder = Mathf.Clamp(pointRT.sizeDelta.x - pointAdd, pointMin, pointMax);
            pointRT.sizeDelta = new Vector2(adder, adder);
        }
    }
}
