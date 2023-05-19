using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//Handle Object Shake & Block Explosions
public class PlayerController : MonoBehaviour
{
    public int bombCount = 0;
    public int shakeCount = 0;

    private Camera mainCam;

    private float yForce = 20f;    //Small bit of Y to help prevent getting stuck on small ledges
    private float xForce = 200f;
    private Rigidbody[] interactables;
    private Outline[] Outlines;
    private UIManager uiManager;

    private bool outlined = false;
    private bool roundEnd = false;

    private void Start() {  //Find all playfield rigidbodies to apply forces
        mainCam = Camera.main;

        interactables = GetComponentsInChildren<Rigidbody>();
        Outlines = GetComponentsInChildren<Outline>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        uiManager.UpdateAbilities(bombCount, shakeCount);

        //Set up Dialog Controller connection to prevent early inputs
    }

    public void ApplyForce(bool right){ //Apply shake to all playfield rigidbodies
        if (shakeCount > 0 && !roundEnd){
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
    }

    private void CheckForBreakables(Vector2 screenPoint){
        if (bombCount > 0 && !roundEnd){
            Ray ray = Camera.main.ScreenPointToRay(screenPoint);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.tag == "Breakable"){
                    BreakBlock(hit.transform.gameObject);
                }
            }
        }
    }

    private void BreakBlock(GameObject clickedObj){
        bombCount--;
        Destroy(clickedObj);
    }

    public void ChangeScene(string newScene){
        SceneManager.LoadScene(newScene);
    }

    public void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Click(InputAction.CallbackContext context){ //Input for Mouse control
        CheckForBreakables(Mouse.current.position.ReadValue());
    }

    public void Tap(InputAction.CallbackContext context){ //Input for Touchscreen control
        CheckForBreakables(Touchscreen.current.primaryTouch.position.ReadValue());
    }

    public bool SetRoundEnd(bool curse){
        if (!roundEnd){
            if (curse){
                uiManager.DisplayCurse();
            } else {
                uiManager.DisplayWin();
            }
            roundEnd = true;
            return true;
        } else {
            return false;
        }
    }
}
