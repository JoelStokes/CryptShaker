using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

//Handle Title Screen UI Controls & Animations
public class TitleController : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LevelMenu;
    public GameObject CreditsMenu;
    public GameObject Title;
    public Animator cameraAnim;

    private string nextScene;
    private float aboveScreen = 500f;
    private float mainStartY;

    private void Start() {
        DisableMenus();

        MainMenu.SetActive(true);
        mainStartY = MainMenu.transform.position.y;
        MainMenu.transform.position = new Vector3(LevelMenu.transform.position.x, -aboveScreen, LevelMenu.transform.position.z);
    }

    private void DisableMenus(){
        MainMenu.SetActive(false);
        LevelMenu.SetActive(false);
        CreditsMenu.SetActive(false);
    }

    public void EndStartSequence(){
        MainMenu.transform.DOMoveY(mainStartY, .75f);
    }

    public void DisplayLevels(){
        DisableMenus();
        LevelMenu.SetActive(true);
    }

    public void DisplayMenu(){
        DisableMenus();
        MainMenu.SetActive(true);
    }

    public void DisplayCredits(){
        DisableMenus();
        CreditsMenu.SetActive(true);
    }

    public void PlayLevel(string scene){
        Title.transform.DOMoveY(aboveScreen*2, 1);
        LevelMenu.transform.DOMoveY(-aboveScreen, 1);
        nextScene = scene;
        cameraAnim.SetTrigger("Start");
    }

    public void ChangeScene(){
        SceneManager.LoadScene(nextScene);
    }
}
