using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private CharacterController player;
    private GameObject cam;
    private GameObject Graphics;

    private float XMove;
    private float YMove;

    private float XMouse;
    private float YMouse;

    private float XCam = 0f;
    private float YCam = 0f;

    public float MouseSens;

    public float WalkSpeed;
    public float SprintSpeed;
    public float crouchSpeed;
    public float JumpHeight;
    public float Gravity;
    
    private float finalGravity;
    public float CurrSpeed;
    private Vector3 LocalVel = Vector3.zero;

    private Vector3 Movedir = Vector3.zero;
    private Vector3 Jumpdir = Vector3.zero;

    private float xform;
    private float yform;
    private float xrot;
    private float yrot;
    private float zrot;

    public float smoothness;
    public bool hidden = false;

    private bool crouch = false;

    void Start()
    {
        player = GetComponent<CharacterController>();
        cam = transform.Find("Camera").gameObject;
        Graphics = GameObject.Find("_GRAPHICS");
        Cursor.lockState = CursorLockMode.Locked;
        CurrSpeed = WalkSpeed;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F3))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        XMouse = Input.GetAxis("Mouse X");
        YMouse = Input.GetAxis("Mouse Y");

        
        Movedir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Movedir = transform.TransformDirection(Movedir) * CurrSpeed;

        if (player.isGrounded)
        {
            Jumpdir = new Vector3(0, 0, 0);
            Jumpdir = transform.TransformDirection(Jumpdir) * CurrSpeed;
            
            if (Input.GetKey(KeyCode.Space))
            {
                Jumpdir.y = JumpHeight;
                crouch = false;
            }
        }
    

        if (Input.GetKey(KeyCode.LeftShift))
        {
            CurrSpeed = SprintSpeed;
            crouch = false;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            crouch = true;
        }
        else if (!Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            CurrSpeed = WalkSpeed;
            crouch = false;
        }
        // JUMP //
        Jumpdir.y -= Gravity * Time.deltaTime;
        if (player.enabled == true)
        {
            player.Move((Movedir + Jumpdir) * Time.deltaTime);
        }

        // CROUCH //

        if (crouch)
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y - 0.1f, 0.75f, 1f), transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, Mathf.Clamp(transform.localScale.y + 0.1f, 0.75f, 1f), transform.localScale.z);
        }

        // CAMERA MOVE // 
        LocalVel = Vector3.Lerp(LocalVel, transform.InverseTransformDirection(player.velocity), 0.2f);
        

        XCam -= XMouse*MouseSens;
        YCam += YMouse*MouseSens;

        this.transform.eulerAngles = new Vector3(0, -XCam, 0)*MouseSens; //body

        if (Input.GetAxis("Vertical") > 0 | Mathf.Abs(Input.GetAxis("Horizontal")) > 0
        && player.velocity.x*player.velocity.x + player.velocity.z*player.velocity.z > 0) // BOBING 
        {
            if (player.isGrounded)
            {
                if (CurrSpeed == WalkSpeed)
                {
                    xform = Mathf.Sin(Time.time * CurrSpeed)/50;
                    yform = Mathf.Sin(Time.time * CurrSpeed*2)/100;
                    float xcurr = Mathf.Sin((Time.time * CurrSpeed)*2)/3;
                    float ycurr  = Mathf.Cos(Time.time * CurrSpeed + 1)/2;
                    float zcurr  = Mathf.Cos(Time.time * CurrSpeed + 1)/2;
                    xrot = Mathf.Lerp(xrot, xcurr, smoothness);
                    yrot = Mathf.Lerp(yrot, ycurr, smoothness);
                    zrot = Mathf.Lerp(zrot, zcurr, smoothness);

                }
                else if (CurrSpeed == SprintSpeed)
                {
                    xform = Mathf.Sin(Time.time * CurrSpeed)/100;
                    yform = Mathf.Sin(Time.time * CurrSpeed*2)/200;
                    float xcurr = Mathf.Sin((Time.time * CurrSpeed)*2)/2;
                    float ycurr = Mathf.Cos(Time.time * CurrSpeed + 1)/1.5f;
                    float zcurr = Mathf.Cos(Time.time * CurrSpeed + 1);
                    xrot = Mathf.Lerp(xrot, xcurr, smoothness);
                    yrot = Mathf.Lerp(yrot, ycurr, smoothness);
                    zrot = Mathf.Lerp(zrot, zcurr, smoothness);

                }
                // if (player.isGrounded)
                // {
                //     cam.transform.Translate(xform, yform, 0); // MOVE BOBING 
                // }
            }
            else
            {
                Mathf.Lerp(xrot, 0, 0.2f);
                Mathf.Lerp(yrot, 0, 0.2f);
                Mathf.Lerp(zrot, 0, 0.2f);
            }
        }
        else
        {
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, new Vector3(0, 0.6f, 0), 0.2f);
            Mathf.Lerp(xrot, 0, 0.2f);
            Mathf.Lerp(yrot, 0, 0.2f);
            Mathf.Lerp(zrot, 0, 0.2f);
        }



        cam.transform.eulerAngles = (new Vector3(-Mathf.Clamp(YCam, -60, 60) + LocalVel.z/4 + xrot, -XCam + yrot, -LocalVel.x/4 + zrot))*MouseSens; // FINAL ROTATION


        
            /*BOBBING*/
        if (player.isGrounded)
        {
            cam.transform.Rotate(Mathf.Cos(Time.time - 0.25f)/100, Mathf.Sin(Time.time)/100, Mathf.Sin(Time.time + 0.25f)/100); 
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject col = collider.gameObject;
        if (col.tag == "HideSpot" && GameManager.utils.playerSeen() < 100)
        {
            hidden = true;
        }
    }
    
    void OnTriggerExit(Collider collider)
    {
        GameObject col = collider.gameObject;
        if (col.tag == "HideSpot")
        {
            hidden = false;
        }
    }
}
