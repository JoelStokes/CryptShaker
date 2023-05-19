using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject ControlUI;
    public GameObject DialogUI;
    public GameObject WinUI;
    public GameObject CurseUI;
    public Image BGUI;

    public TextMeshProUGUI BombText;
    //public TextMeshProUGUI ShakeText;

    private PlayerController playerController;

    private Vector3 dialogStartPos;
    private Vector3 controlStartPos;
    private Vector3 winStartScale;

    private float speed = .75f;

    void Awake(){
        ControlUI.SetActive(true);
        DialogUI.SetActive(true);
        WinUI.SetActive(true);
        CurseUI.SetActive(true);

        dialogStartPos = DialogUI.transform.position;
        controlStartPos = ControlUI.transform.position;
        winStartScale = WinUI.transform.localScale;

        WinUI.transform.localScale = Vector3.zero;
        CurseUI.transform.localScale = Vector3.zero;
        ControlUI.transform.position = dialogStartPos;
    }

    public void ShowDialog(){
        BGUI.DOFade(.8f, speed);
        DialogUI.transform.DOMove(transform.position, speed);
    }

    public void HideDialog(){
        BGUI.DOFade(0, speed);
        DialogUI.transform.DOMove(dialogStartPos, speed);
        ControlUI.transform.DOMove(controlStartPos, speed);
    }

    public void DisplayWin(){
        ControlUI.transform.DOMove(dialogStartPos, speed);
        WinUI.transform.DOScale(winStartScale, speed);
    }

    public void DisplayCurse(){
        ControlUI.transform.DOMove(dialogStartPos, speed);
        CurseUI.transform.DOScale(winStartScale, speed);
    }

    public void UpdateAbilities(int bombs, int shakes){
        BombText.text = bombs.ToString();
        //ShakeText.text = shakes.ToString();   //Removed for now, decide if it's worth counting?
    }
}
