using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{
    public void ButtonExit()
    {
        StartCoroutine(ButtonExitCor());
    }
    private IEnumerator ButtonExitCor()
    {
        yield return new WaitForSeconds(0);

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
