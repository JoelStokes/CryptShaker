using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudio : MonoBehaviour
{
    private void Awake() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");
        
        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "Title"){
            Destroy(gameObject);
        }
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Title"){
            Destroy(gameObject);
        }
    }
}
