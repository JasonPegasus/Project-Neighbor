using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeighborVision : MonoBehaviour
{
    private NeighborController ncontrol;
    public GameObject head;
    public int PlayerVisible;
    public List<GameObject> items = new List<GameObject>();

    public LayerMask targetLayer;
    public LayerMask wallLayer;

    void Start()
    {
        StartCoroutine(UpdateVision());
        ncontrol = GetComponent<NeighborController>();

        int count = 0;
        foreach(Transform i in GameManager.utils.items.transform)
        {
            count++;
            items.Add(i.gameObject);
        }
    }

    void Update()
    {
        Debug.DrawRay(head.transform.position, head.transform.forward * range, Color.red);
    }

    private IEnumerator UpdateVision()
    {
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while (true)
        {
            yield return delay;
            Vision();
        }
    }

    public float range;
    public float FOV_see;
    public float FOV_suspect;
    public float suspectRange;
    public float touchRange;

    private Vector3 targetDirection;
    private float targetDistance;

    public void Vision()
    {
        Collider[] scanArea = Physics.OverlapSphere(head.transform.position, range, targetLayer);
        // Escanea el area entera en esfera, en busca de sospechas/jugador

        if (scanArea.Length != 0) // Si se encontró algo sospechoso/jugador en el area
        {
            for (int i = 1; i < scanArea.Length; i++)
            {
                Transform target = scanArea[i].transform; // Toma lo primero en encontrar (SE DEBE CAMBIAR POSTERIORMENTE A UN ITERATOR POR TABLA)

                targetDirection = (target.position - head.transform.position).normalized; // Da la direccion entre ese objeto y el vecino
                targetDistance = Vector3.Distance(transform.position, target.position); // Da la distancia entre el objeto y el vecino

                if (!Obstructed()) // Si no hay pared
                {
                    if (targetDistance > touchRange) // Si no lo toca
                    {
                        if (onFov(FOV_see))
                        {
                            seeObject(100, target.gameObject);
                        }
                        else
                        {
                            if (onFov(FOV_suspect) && targetDistance < suspectRange)
                            {
                                seeObject(50, target.gameObject);
                            }
                            else
                            seeObject(0, target.gameObject);
                        }
                    }
                    else
                    seeObject(100, target.gameObject);
                }
                else
                seeObject(0, target.gameObject);
            }
        }
        else
        seeObject(0, GameManager.utils.player);
    }

    public bool Obstructed()
    {
        if(Physics.Raycast(head.transform.position, targetDirection, targetDistance, wallLayer))// Comprueba que no hay nada tapando al objeto
        {
            return true;
        }
        else
        return false;
    }

    public bool onFov(float fov)
    {
        if(Vector3.Angle(head.transform.forward, targetDirection) < fov/2)
        {
            return true;
        }
        else
        return false;
    }

    private float seeCool = 0;
    public void seeObject(int vis, GameObject obj)
    {
        if (obj == GameManager.utils.player)
        {
            seePlayer(vis);
        }
        else if((obj.transform.tag == "Item" | obj.transform.parent.tag == "Item") && vis > 0)
        {
            for (int i=1; i < items.Count; i++)
            {
                GameObject item = obj;
                if (obj.transform.tag != "Item")
                {item = obj.transform.parent.gameObject;}

                float dist = Vector3.Distance(item.transform.position, item.GetComponent<ItemPhysics>().lastpos);
                if (dist > 0.3f && item.GetComponent<ItemPhysics>().lasttouch == GameManager.utils.player)
                {
                    ncontrol.SuspectAt(item.transform.position);
                }
            }
        }
    }

    private int lastamount = 0;

    public void seePlayer(int vis)
    {
        PlayerVisible = vis;
        seeCool--;
        bool hidden = GameManager.utils.playerHidden();
        
        if (PlayerVisible >= 100 && !hidden)
        {
            ncontrol.StartChase();
        }
        if (PlayerVisible == 50 && seeCool <= 0)
        {
            ncontrol.SuspectAt(ncontrol.Player.transform.position);
            seeCool = 10;
        }
        if (PlayerVisible == 0 && lastamount == 100)
        {
            
        }
        lastamount = vis;
    }
}
