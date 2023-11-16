using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NeighborController : MonoBehaviour
{
    public Vector3 target;
    public GameObject Player;
    public string MODE;
    public float HouseRadius;
    private float totaldist;

    private Rigidbody rb;
    private GameObject neicam;
    private NavMeshAgent nav;
    private Animator anim;
    private new NeighborSounds audio;
    private GameObject house;

    public float SpeedWalk;
    public float SpeedHunt;
    public float SpeedRun;
    private float moveSpeed;
    public bool moving = false;


    void Start()
    {
        // declarations //
        nav = GetComponent<NavMeshAgent>();
        neicam = GetComponent<NeighborVision>().head;
        rb = GetComponent<Rigidbody>();
        Player = GameObject.Find("PLAYER");
        anim = transform.Find("Model").gameObject.GetComponent<Animator>();
        audio = transform.Find("Sounds").gameObject.GetComponent<NeighborSounds>();
        house = GameObject.Find("House").gameObject;

        // tweaks //
        transform.Find("Model").localPosition = new Vector3(0, -0.5f, -0.25f);
        moveSpeed = SpeedWalk;
        MODE = "Normal";
        target = transform.position;
    }

    void Update()
    {
        if (target != null){nav.destination = target;}
        anim.SetBool("IsMoving", moving);
        NeighborInteract();
        MovementState();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        float distx = house.transform.Find("Center").position.x - Player.transform.position.x;
        float distz = house.transform.Find("Center").position.z - Player.transform.position.z;
        totaldist = Mathf.Sqrt(distx*distx + distz*distz);
    }



    public void Hear(Vector3 pos, float radius)
    {
        float ndistx = transform.position.x - pos.x;
        float ndistz = transform.position.z - pos.z;
        float neighbordist = Mathf.Sqrt(ndistx*ndistx + ndistz*ndistz);
        if (neighbordist <= radius)
        {
            SuspectAt(pos);
        }
    }


    // states: 0 idle, 1 walk, 2 Hunt, 3 run
    public int moveState = 0;
    public void MovementState()
    {
        if (nav.velocity.magnitude > 0) {moving = true;} else {moving = false;}
            if (moving)
            {
                anim.SetInteger("Movement", moveState);
                if (moveState == 1)
                {moveSpeed = SpeedWalk;}
                if (moveState == 2)
                {moveSpeed = SpeedHunt;}
                if (moveState == 3)
                {moveSpeed = SpeedRun;}
            }
        else
        {anim.SetInteger("Movement", 0);}
        nav.speed = moveSpeed;
    }

    public int SuspectTimes = 0;
    public void SuspectAt(Vector3 point)
    {
        if (MODE != "Chase" && totaldist < HouseRadius)
        {
            SuspectTimes++;
            if (Random.Range(1,2) == 1)
            {audio.PlaySound(audio.hm1);}
            else
            {audio.PlaySound(audio.hm2);}
            target = point;
            moveState = 2;

            if (SuspectTimes >= 3)
            {
                MODE = "HUNT";
            }
        }
    }

    public void HearLook(Vector3 pos, float radius)
    {
        float ndistx = transform.position.x - pos.x;
        float ndistz = transform.position.z - pos.z;
        float neighbordist = Mathf.Sqrt(ndistx*ndistx + ndistz*ndistz);
        if (neighbordist <= radius)
        {
            LookAt(Player.transform.position);
        }
    }
    
    public void LookAt(Vector3 point)
    {
        if (MODE != "Chase")
        {
            nav.destination = point;
            moveState = 0;
        }
    }

    public void StartChase()
    {
        if (MODE != "Chase" && totaldist < HouseRadius && !catched)
        {
            MODE = "Chase";
            target = Player.transform.position;
            StartCoroutine(ChaseUpdate());
            GetComponent<AudioSource>().Play();
            audio.PlaySound(audio.surprise);
        }
    }
    

    public void StopChase()
    {
        if (MODE == "Chase")
        {
            MODE = "Normal";
            target = transform.position;
            StopCoroutine(ChaseUpdate());
            GetComponent<AudioSource>().Stop();
            moveState = 1;
        }
    }

    public float chasetime = 10;
    private bool predicted;
    IEnumerator ChaseUpdate()
    {
        while (MODE == "Chase")
        {
            yield return new WaitForSeconds(0.5f);
            moveState = 3;
            if (GameManager.utils.playerSeen() == 100)
            {
                target = Player.transform.position;
                predicted = false;
            }
            else if(saw != transform.position)
            {
                PlayerController plr = Player.GetComponent<PlayerController>();
                if (!predicted)
                StartCoroutine(Predict());
                if(plr.CurrSpeed == plr.SprintSpeed)
                {
                    target = GetRandomPos(5);
                }
            }
            if (totaldist > HouseRadius)
            {
                StopChase();
            }
        }
    }

    IEnumerator Predict()
    {
        print("PREDICT");
        yield return new WaitForSeconds(2f);
        if (GameManager.utils.playerSeen() < 100 | GameManager.utils.playerHidden())
        {
            target = GetNearestWaypoint(Player.transform.position + Player.GetComponent<CharacterController>().velocity).transform.position;
        }
        predicted = true;
    }

    private Vector3 lastrand = Vector3.zero;

    public Vector3 GetRandomPos(float radio)
    {
        Vector3 pp = Player.transform.position;
        float ran = Random.Range(-100,100)/100*radio;
        Vector3 pos = new Vector3(pp.x + Mathf.Sin(ran),pp.y, pp.z + Mathf.Cos(ran));
        lastrand = pos;
        if(pos != lastrand)
        {return pos;}
        else
        {return lastrand;}
    }

    public GameObject GetNearestWaypoint(Vector3 pos)
    {
        float minDist = Mathf.Infinity;
        GameObject way = null;
        foreach (Transform i in GameObject.Find("Waypoints").transform)
        {
            float dist = Vector3.Distance(i.position, pos);
            if (dist < minDist)
            {
                minDist = dist;
                way = i.gameObject;
            }
        }
        return way;
    }


    private Vector3 saw;
    public IEnumerator LastSaw()
    {
        yield return new WaitForSeconds(2f);
        saw = Player.transform.position;
    }
    
    
    private bool catched = false;
    IEnumerator Catch()
    {
        if (!catched && MODE == "Chase")
        {
            catched = true;
            transform.Find("CatchSound").gameObject.GetComponent<AudioSource>().Play();
            StopChase();
            yield return new WaitForSeconds(0.1f);
            target = transform.position;
            moveState = 0;
        }
    }

    // ENVIROMENT //
    private void NeighborInteract()
    {
        RaycastHit hit;
        if (Physics.Raycast(neicam.transform.position, neicam.transform.forward, out hit, 1f, Physics.AllLayers))
        {
            GameObject ho = hit.transform.gameObject;
            OpenDoor(ho);
            OpenCloset(ho);
        }   
    }

    private void OpenDoor(GameObject ho)
    {
        DoorSystem ds = ho.GetComponent<DoorSystem>();
        if (ho.transform.tag == "Interactable" && ds != null && !ds.isopen)
        {
            ds.Execute(ho, 0.75f, 0, true);
        }
    }

    private void OpenCloset(GameObject ho)
    {
        ClosetDoor cd = ho.GetComponent<ClosetDoor>();
        if (ho.transform.tag == "Interactable" && cd != null)
        {
            if (MODE == "Chase" | MODE == "Hunt")
            cd.Execute(true);
        }
    }
    void OnCollisionEnter(Collision obj)
    {
        GameObject hit = obj.gameObject;
        GlassSystem gs = hit.GetComponent<GlassSystem>();
        if (gs != null)
        {
            BreakWindow(hit, gs);
        }

        if (hit == Player)
        {
            StartCoroutine(Catch());
        }
    }
    
    private GameObject lastway = null;
    private GameObject lastway2 = null;

    void OnTriggerEnter(Collider collider)
    {
        GameObject col = collider.gameObject;
        if (col.layer == LayerMask.NameToLayer("Waypoint") && col != lastway && col != lastway2 && (col == GetNearestWaypoint(Player.transform.position) | col == GetNearestWaypoint(transform.position)))
        {
            lastway2 = lastway;
            GetNearestWaypoint(transform.position);
            lastway = col;
        }
    }

    private void BreakWindow(GameObject ho, GlassSystem gs)
    {
        if (gs != null)
        {
            gs.Break(rb.velocity/100, transform.position);
        }
    }
}
