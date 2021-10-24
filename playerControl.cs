using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    [SerializeField] Vector2 sensitibity;
    [SerializeField] Vector2 limit=new Vector2(0,325);
    [SerializeField] Vector3 cameraPosition = new Vector3(0, 0, 0);
    [SerializeField] Vector3 cameraRotation = new Vector3(0, 0, 0);
    [SerializeField] float speed;
    [SerializeField] Vector2 nearCameraSpeed=new Vector2(0.12f,0.08f);
    [SerializeField] new Transform camera=null;
    [SerializeField] Transform axis=null;
    [SerializeField] private bool cursorIsBlocked=true;
    [SerializeField] private bool isFPS=false;
    private Vector2 rotation;
    private Vector3 basePosition= new Vector3(0,0,0);
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
    private void thirdpersonCamera(Transform axis){
        Vector2 velocity = MouseLook()*sensitibity;
        rotation += velocity*Time.deltaTime;
        axis.localEulerAngles = new Vector3(0,rotation.x,rotation.y);
        rotation.y = Mathf.Clamp(rotation.y,limit.x,limit.y);
        if(axis.localEulerAngles.z>0 && axis.localEulerAngles.z<268){
            Debug.Log(camera.localPosition);
            camera.localPosition = new Vector3(axis.localEulerAngles.z*nearCameraSpeed.y,axis.localEulerAngles.z*nearCameraSpeed.x,0);
        }else{
            camera.localPosition = Vector3.Lerp(camera.localPosition,basePosition,0.1f);
        }
    }
    
    private void FixedUpdate(){
        Control();
        thirdpersonCamera(axis);
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