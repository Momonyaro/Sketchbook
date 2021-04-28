using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Image transitionScreen;
    bool fadeOut;
    float fade = 0.0f;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        transitionScreen.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        fadeOut = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!fadeOut)
        {
            fade += Time.deltaTime * 2.0f;
            if (fade > 1.0f)
                fade = 1.0f;
        }
        else
            fade -= Time.deltaTime * 2.0f;

        transitionScreen.color = new Color(1.0f, 1.0f, 1.0f, fade);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneChanged;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneChanged;
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        fadeOut = true;
        Destroy(gameObject, 1.0f);
    }
}
