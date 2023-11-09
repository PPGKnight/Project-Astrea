using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ExitGame : MonoBehaviour
{
    public void ButtonExit()
    {
        StartCoroutine(ButtonExitCor());
    }
    private IEnumerator ButtonExitCor()
    {
        Debug.Log("GoTouchGrass");
        yield return new WaitForSeconds(0);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
