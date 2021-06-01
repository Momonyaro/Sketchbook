using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [Tooltip("The canvas with the transition object that gets spawned when a scene transition is triggered (i.e. the white fade-in)")]
    public GameObject blackScreenCanvas;
    public string scene;
    bool triggered;

    void Start()
    {
        triggered = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !triggered)
            StartSceneChange();
    }
    public void StartSceneChange()
    {
        triggered = true;
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        Instantiate(blackScreenCanvas, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(scene);
    }
}
