using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//Handle Collectible's end of round animations for win or lose & Ensure no Z movement
public class CollectibleController : MonoBehaviour
{
    public enum RotationAxis{
        x, y, z
    };

    public bool cursed = false;
    public RotationAxis rotationAxis;   //Used for level end animation
    public float yAdjust = 0;

    private float zAdjust = -1.5f;
    private float scaleMult = 2.5f;
    private float zPos;
    private Quaternion startRotation;
    private bool roundEnd = false;

    private void Start() {
        zPos = transform.position.z;
        startRotation = transform.rotation;
    }

    private void Update() {
        if (!roundEnd){
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);
        }
    }

    void OnTriggerEnter(Collider other){
        if (other.tag == "Finish"){
            PlayerController playerController = GameObject.Find("Playfield").GetComponent<PlayerController>();
            GameObject UIObj;
            roundEnd = true;

            if (cursed){
                if (!playerController.SetRoundEnd(true)){
                    return;
                }
                UIObj = GameObject.Find("Curse Textbox");
            } else {
                if (!playerController.SetRoundEnd(false)){
                    return;
                }
                UIObj = GameObject.Find("Win Textbox");
            }

            Rigidbody rigi = GetComponent<Rigidbody>();

            rigi.angularVelocity = Vector3.zero;
            rigi.isKinematic = true;
            transform.rotation = startRotation;

            gameObject.layer = LayerMask.NameToLayer("UI");

            DOTween.Init();
            transform.DOMove(new Vector3(UIObj.transform.position.x, UIObj.transform.position.y + yAdjust, UIObj.transform.position.z + zAdjust), 1);
            transform.DOScale(transform.localScale * scaleMult, 1);

            Vector3 rotation = new Vector3();
            switch (rotationAxis.ToString()){
                case "x":
                    rotation = new Vector3(360,0,0);
                    break;
                case "y":
                    rotation = new Vector3(0,360,0);
                    break;
                case "z":
                    rotation = new Vector3(0,0,360);
                    break;                
            }
            transform.DORotate(rotation, 2f, RotateMode.LocalAxisAdd).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
}
