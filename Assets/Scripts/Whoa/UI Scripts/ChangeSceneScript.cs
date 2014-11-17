using UnityEngine;
using System.Collections;

public class ChangeSceneScript : MonoBehaviour {

    public void ChangeScene(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
