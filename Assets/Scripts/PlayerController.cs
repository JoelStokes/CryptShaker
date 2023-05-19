using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float bombCount = 0;
    public float shakeCount = 0;

    private Camera mainCam;

    private float yForce = 20f;    //Small bit of Y to help prevent getting stuck on small ledges
    private float xForce = 200f;
    private Rigidbody[] interactables;
    private Outline[] Outlines;

    private bool outlined = false;

    private void Start() {  //Find all playfield rigidbodies to apply forces
        mainCam = Camera.main;

        interactables = GetComponentsInChildren<Rigidbody>();
        Outlines = GetComponentsInChildren<Outline>();
    }

    public void ApplyForce(bool right){ //Apply shake to all playfield rigidbodies
        if (shakeCount > 0){
            float newX;
            if (right){
                newX = xForce;
            } else {
                newX = -xForce;
            }

            for (int i=0; i<interactables.Length; i++){
                interactables[i].AddForce(new Vector3(newX + interactables[i].velocity.x, yForce + interactables[i].velocity.y, 0));
            }

            shakeCount--;
            //Update Shake Count UI
        }
    }

    public void OutlineBombs(){
        outlined = !outlined;

        for (int i=0; i<Outlines.Length; i++){
            if (outlined){
                Outlines[i].OutlineWidth = 2;
            } else {
                Outlines[i].OutlineWidth = 0;
            }
        }

        bombCount--;
        //Update Bomb Count UI
    }

    private void CheckForBreakables(Vector2 screenPoint){
        if (bombCount > 0){
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == "Breakable"){
                    BreakBlock(hit.transform.gameObject);
                    bombCount--;
                }
            }
        }
    }

    private void BreakBlock(GameObject clickedObj){
        Destroy(clickedObj);
    }

    public void Click(InputAction.CallbackContext context){ //Input for Mouse control
        CheckForBreakables(Mouse.current.position.ReadValue());
    }

    public void Tap(InputAction.CallbackContext context){ //Input for Touchscreen control
        CheckForBreakables(Touchscreen.current.primaryTouch.position.ReadValue());
    }
}
