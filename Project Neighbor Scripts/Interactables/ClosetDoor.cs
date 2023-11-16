using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetDoor : MonoBehaviour
{
    public void Execute(bool byneighbor)
    {
        transform.parent.transform.parent.gameObject.GetComponent<ClosetSystem>().StartCoroutine("Change");
    }
}
