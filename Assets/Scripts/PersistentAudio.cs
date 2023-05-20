using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private bool audioBegin = false;

    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        
        if (!audioBegin) {
            if (objs.Length > 1)
            {
                Destroy(gameObject);
            }

            audioSource.Play ();
            audioBegin = true;

            DontDestroyOnLoad(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Title"){
            Destroy(gameObject);
        }
    }
}
