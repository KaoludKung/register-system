using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        PlaySound.instance.soundPlay();
        StartCoroutine(WaitAndLoadScene(sceneName));
    }

    private IEnumerator WaitAndLoadScene(string sceneName)
    {
        yield return new WaitForSeconds(PlaySound.instance.audioClip.length);
        SceneManager.LoadScene(sceneName);
    }
}
