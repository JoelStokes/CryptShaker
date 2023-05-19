using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CollectibleController : MonoBehaviour
{
    public enum RotationAxis{
        x, y, z
    };

    public bool cursed = false;
    public RotationAxis rotationAxis;   //Used for level end animation

    private float zAdjust = -1.5f;
    private float scaleMult = 2f;

    void OnTriggerEnter(Collider other){
        if (other.tag == "Finish"){
            PlayerController playerController = GameObject.Find("Playfield").GetComponent<PlayerController>();
            GameObject UIObj;

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

            GetComponent<Rigidbody>().isKinematic = true;

            transform.position = new Vector3(UIObj.transform.position.x, transform.position.y, UIObj.transform.position.z + zAdjust);
            gameObject.layer = LayerMask.NameToLayer("UI");

            DOTween.Init();
            transform.DOMove(new Vector3(UIObj.transform.position.x, UIObj.transform.position.y, UIObj.transform.position.z + zAdjust), 1);
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
