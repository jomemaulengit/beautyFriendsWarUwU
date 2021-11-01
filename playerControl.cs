using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] Vector2 nearCameraSpeed=new Vector2(0.12f,0.08f);
    [SerializeField] Vector2 sensitibity;
    [SerializeField] Vector2 limit=new Vector2(0,325);
    [SerializeField] Vector3 cameraPosition = new Vector3(0, 0, 0);
    [SerializeField] Vector3 cameraRotation = new Vector3(0, 0, 0);
    [SerializeField] new Transform camera=null;
    [SerializeField] Transform axis=null;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private bool cursorIsBlocked=true;
    [SerializeField] private bool isFPS=false;
    
    private bool isGrounded=true;
    private Vector2 rotation;
    private Vector3 basePosition= new Vector3(0,0,0);
    private Rigidbody myrb;
    private Collider[] groundCollision;
    private float maxVarticalDegrees=90;
    


// =======================START====================================================
    void Start(){
        if(cursorIsBlocked){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible=false;
        }
        Application.targetFrameRate = 62;
        myrb = GetComponent<Rigidbody>();
    }
// =======================FRAME====================================================
    private void FixedUpdate(){
        Control();
        thirdpersonCamera(axis);
        Grounded();
        Debug.Log(isGrounded);
    }

// =======================CONTROL====================================================
    private void Grounded(){
        groundCollision = Physics.OverlapSphere(groundCheck.position, 0.5f,groundLayer);
        isGrounded = groundCollision.Length > 0;
    }
    private void thirdpersonCamera(Transform axis){
        Vector2 velocity = MouseLook()*sensitibity;
        rotation += velocity*Time.deltaTime;
        axis.localEulerAngles = new Vector3(0,rotation.x,rotation.y);
        rotation.y = Mathf.Clamp(rotation.y,limit.x,limit.y);
        if(axis.localEulerAngles.z>0 && axis.localEulerAngles.z<268){
            camera.localPosition = new Vector3(axis.localEulerAngles.z*nearCameraSpeed.y,axis.localEulerAngles.z*nearCameraSpeed.x,0);
        }else{
            camera.localPosition = Vector3.Lerp(camera.localPosition,basePosition,0.1f);
        }
        axis.localPosition = transform.position;
        Control();
    }

    private void fpsCamera(){
        camera.localPosition = new Vector3(0,0,0);
        Vector2 velocity = MouseLook()*sensitibity;
        rotation += velocity*Time.deltaTime;
        rotation.y = ClampVerticalAngles(rotation.y);
        transform.localEulerAngles= new Vector3(0,rotation.x,0);
        camera.localEulerAngles = new Vector3(-rotation.y,0,0);
    }
// =======================MOUSE-CONTROL====================================================
    private float ClampVerticalAngles(float angle){
        return Mathf.Clamp(angle,-maxVarticalDegrees,maxVarticalDegrees);
    }
    private Vector2 MouseLook(){
        Vector2 input = new Vector2(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y")
        );
        return input;
    }
// =======================KEYS-CONTROL====================================================
    private void switchCamera(Transform axis){
        if(Input.GetKeyDown(KeyCode.C)){
            isFPS=!isFPS;
        }
    }
    private void move(Vector3 rotation,float angle){
        myrb.velocity = transform.right*speed;
        transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0,rotation.x+angle,0),0.15f);
}

    private void Control(){
        switchCamera(axis);
        if (Input.GetKey(KeyCode.W)){
            move(rotation,0f);
        }
        if (Input.GetKey(KeyCode.S)){
            move(rotation,-180);
        }
        if (Input.GetKey(KeyCode.D)){
            move(rotation,90);
        }
        if (Input.GetKey(KeyCode.A)){
            move(rotation,-90);
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded){
            myrb.AddForce(Vector3.up*jumpForce,ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Tab)){
            if(cursorIsBlocked==true)
              {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible=true;
                cursorIsBlocked=false;
              }else{
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible=false;
                cursorIsBlocked=true;
              }
        }
    }
}