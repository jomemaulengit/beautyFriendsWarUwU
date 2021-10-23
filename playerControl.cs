using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl : MonoBehaviour
{
    [SerializeField] Vector2 sensitibity;
    [SerializeField] float speed;
    [SerializeField] new Transform camera=null;
    [SerializeField] private bool cursorIsBlocked=true;
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
    private void Control(){
        if (Input.GetKey(KeyCode.W)){
            myrb.velocity = new Vector3(0,0,speed);
        }
        if (Input.GetKey(KeyCode.S)){
            myrb.velocity = new Vector3(0,0,-speed);
        }
        if (Input.GetKey(KeyCode.D)){
            myrb.velocity = new Vector3(0,0,speed);
        }
        if (Input.GetKey(KeyCode.A)){
            myrb.velocity = new Vector3(0,0,-speed);
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
    private void FixedUpdate(){
        Vector2 velocity = MouseLook()*sensitibity;
        rotation += velocity*Time.deltaTime;
        rotation.y = ClampVerticalAngles(rotation.y);
        transform.localEulerAngles= new Vector3(0,rotation.x,0);
        camera.localEulerAngles = new Vector3(-rotation.y,0,0);
        Control();
    }
}