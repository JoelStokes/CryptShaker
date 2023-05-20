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
    public AudioClip clickSFX;
    public AudioClip backSFX;

    private string nextScene;
    private float aboveScreen = 500f;
    private float mainStartY;
    private float volume = .3f;

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
        AudioSource.PlayClipAtPoint(clickSFX, Camera.main.gameObject.transform.position, volume);
    }

    public void DisplayMenu(){
        DisableMenus();
        MainMenu.SetActive(true);
    }

    public void DisplayCredits(){
        DisableMenus();
        CreditsMenu.SetActive(true);
        AudioSource.PlayClipAtPoint(clickSFX, Camera.main.gameObject.transform.position, volume);
    }

    public void PlayLevel(string scene){
        Title.transform.DOMoveY(aboveScreen*2, 1);
        LevelMenu.transform.DOMoveY(-aboveScreen, 1);
        nextScene = scene;
        cameraAnim.SetTrigger("Start");
        AudioSource.PlayClipAtPoint(clickSFX, Camera.main.gameObject.transform.position, volume);
    }

    public void ChangeScene(){
        SceneManager.LoadScene(nextScene);
    }
}
