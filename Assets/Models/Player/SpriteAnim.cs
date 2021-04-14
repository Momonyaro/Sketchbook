using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SpriteAnim : MonoBehaviour
{
    public float FPS = 15f;

    // Start is called before the first frame update
    void Start() {
        NextFrame();
    }

    void NextFrame() {
        CancelInvoke("NextFrame");
        GameObject onChild = null;
        GameObject nextChild = null;
        foreach (Transform child in transform) {
            GameObject obj = child.gameObject;
            var meshfilter = child.GetComponent<Renderer>();
            if (meshfilter != null) {
                if(obj.activeSelf && !onChild) {
                    onChild = obj;
                    obj.SetActive(false);
                    continue;
                }
                if(!obj.activeSelf && onChild != null && nextChild == null) {
                    nextChild = obj;
                    obj.SetActive(true);
                    continue;
				} else if(obj.activeSelf) {
                    obj.SetActive(false);
                }
            }
        }
        if(!nextChild) {
            transform.GetChild(0).gameObject.SetActive(true);
		}
        Invoke("NextFrame", 1/FPS);

    }

    // Update is called once per frame
    void Update()
    {
    }
}
