using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject neighbor;
    public GameObject player;
    public GameObject cam;
    public NeighborController nc;
    public NeighborVision nv;
    public InventorySystem inv;
    public GameObject items;
    public GameObject graphics;

    public static GameManager utils {get;private set;}

    void Awake()
    {
        if (utils != null && utils != this)
        {Destroy(this);}
        else
        {utils = this;}
        neighbor = GameObject.Find("Neighbor");
        player = GameObject.Find("PLAYER");
        cam = player.transform.Find("Camera").gameObject;
        nc = neighbor.GetComponent<NeighborController>();
        nv = neighbor.GetComponent<NeighborVision>();
        inv = GameObject.Find("_INVENTORY").gameObject.GetComponent<InventorySystem>();
        items = GameObject.Find("_ITEMS");
        graphics = GameObject.Find("_GRAPHICS");
    }

    void Start()
    {
    }

    public bool playerHidden()
    {
        return player.GetComponent<PlayerController>().hidden;
    }

    public int playerSeen()
    {
        return nv.PlayerVisible;
    }
}

