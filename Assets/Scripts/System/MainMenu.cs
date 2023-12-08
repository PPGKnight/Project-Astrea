using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public void NewGame() => LoadingScreenManager.Instance.LoadScene("Inn");

    // TODO: Save and Continue
    // public void ContinueGame();

    public void ButtonExit() => StartCoroutine(ButtonExitCor());
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
