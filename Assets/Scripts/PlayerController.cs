using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using DG.Tweening;

//Handle Object Shake & Block Explosions
public class PlayerController : MonoBehaviour
{
    public int bombCount = 0;
    public int shakeCount = 0;
    public GameObject blastPrefab;

    public AudioClip explosionSFX;
    public AudioClip negativeSFX;
    public AudioClip thwackSFX;
    private float thwackVolume = .5f;

    private GameObject mainCam;
    private float cameraShakeForce = .25f;

    private float yForce = 20f;    //Small bit of Y to help prevent getting stuck on small ledges
    private float xForce = 200f;
    private Rigidbody[] interactables;
    private Outline[] Outlines;
    private UIManager uiManager;

    private bool outlined = false;
    private bool roundEnd = false;
    private bool roundStart = false;

    private void Start() {  //Find all playfield rigidbodies to apply forces
        mainCam = Camera.main.gameObject;

        interactables = GetComponentsInChildren<Rigidbody>();
        Outlines = GetComponentsInChildren<Outline>();

        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        uiManager.UpdateAbilities(bombCount, shakeCount);

        //Set up Dialog Controller connection to prevent early inputs
    }

    public void ApplyForce(bool right){ //Apply shake to all playfield rigidbodies
        if (shakeCount > 0 && !roundEnd){
            AudioSource.PlayClipAtPoint(thwackSFX, mainCam.transform.position, thwackVolume);

            float newX;
            if (right){                
                mainCam.transform.DOShakePosition(.25f, new Vector3(cameraShakeForce, 0, 0));
                newX = xForce;
            } else {
                mainCam.transform.DOShakePosition(.25f, new Vector3(-cameraShakeForce, 0, 0));
                newX = -xForce;
            }

            for (int i=0; i<interactables.Length; i++){
                interactables[i].AddForce(new Vector3(newX + interactables[i].velocity.x, yForce + interactables[i].velocity.y, 0));
            }

            //shakeCount--;
            //Shake Limit not being used for this build, but would be added as a later feature for further puzzle complexity/challenge
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
        if (!roundEnd && roundStart){
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
        if (bombCount > 0 ){
            bombCount--;
            uiManager.UpdateAbilities(bombCount, shakeCount);
            Instantiate(blastPrefab, clickedObj.transform.position, Quaternion.identity);
            AudioSource.PlayClipAtPoint(explosionSFX, clickedObj.transform.position);
            Destroy(clickedObj);            
        } else {
            AudioSource.PlayClipAtPoint(negativeSFX, clickedObj.transform.position);
        }
    }

    public void ChangeScene(string newScene){
        SceneManager.LoadScene(newScene);
    }

    public void ReloadScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Click(InputAction.CallbackContext context){ //Input for Mouse control
        if (context.started){
            CheckForBreakables(Mouse.current.position.ReadValue());
        }
    }

    public void Tap(InputAction.CallbackContext context){ //Input for Touchscreen control
        if (context.started){
            CheckForBreakables(Touchscreen.current.primaryTouch.position.ReadValue());
        }
    }

    public void SetRoundStart(){
        roundStart = true;
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
