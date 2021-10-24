using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    [SerializeField] Vector2 sensitibity;
    [SerializeField] Vector3 cameraPosition = new Vector3(0, 0, 0);
    [SerializeField] Vector3 cameraRotation = new Vector3(0, 0, 0);
    [SerializeField] float speed;
    [SerializeField] new Transform camera=null;
    [SerializeField] Transform axis=null;
    [SerializeField] private bool cursorIsBlocked=true;
    [SerializeField] private bool isFPS=false;
    private Vector2 rotation;
    private Rigidbody myrb;
    private float maxVarticalDegrees=90;
    void Start(){
        if(cursorIsBlocked){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible=false;
        }
        Application.targetFrameRate = 62;
        myrb = GetComponent<Rigidbody>();
    }
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
    private void fpsCamera(){
        camera.localPosition = new Vector3(0,0,0);
        Vector2 velocity = MouseLook()*sensitibity;
        rotation += velocity*Time.deltaTime;
        rotation.y = ClampVerticalAngles(rotation.y);
        transform.localEulerAngles= new Vector3(0,rotation.x,0);
        camera.localEulerAngles = new Vector3(-rotation.y,0,0);
    }
    private void switchCamera(Transform axis){
        if(Input.GetKeyDown(KeyCode.C)){
            isFPS=!isFPS;
        }
    }
    private void thirdpersonCamera(){
        // if(camera.localEulerAngles.x<10){
        //     cameraPosition.z+=camera.localEulerAngles.x;
        // }
        camera.localPosition = cameraPosition;
        camera.localEulerAngles = cameraRotation;
        // Vector2 velocity = MouseLook()*sensitibity;
        // rotation += velocity*Time.deltaTime;
        // rotation.y = ClampVerticalAngles(rotation.y);
        // transform.localEulerAngles= new Vector3(0,rotation.x,0);
        // camera.localEulerAngles = new Vector3(-rotation.y,0,0);

    }
    
    private void FixedUpdate(){
        Control();
        if(!isFPS){fpsCamera();}else{thirdpersonCamera();}
    }
    private void Control(){
        switchCamera(axis);
        if (Input.GetKey(KeyCode.W)){
            myrb.AddRelativeForce(new Vector3(0,0,speed));
        }
        if (Input.GetKey(KeyCode.S)){
            myrb.AddRelativeForce(new Vector3(0,0,-speed));
        }
        if (Input.GetKey(KeyCode.D)){
            myrb.AddRelativeForce(new Vector3(speed,0,0));
        }
        if (Input.GetKey(KeyCode.A)){
            myrb.AddRelativeForce(new Vector3(-speed,0,0));
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