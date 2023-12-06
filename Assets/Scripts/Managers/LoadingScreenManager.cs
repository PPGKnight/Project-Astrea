using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{
    LoadingScreenManager instance;
    public LoadingScreenManager Instance { get { return instance; } }

    [SerializeField] GameObject screen;
    [SerializeField] Image loadingFillAmount;

    private void Awake()
    {
        if (instance == null) {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
            Destroy(this.gameObject);

    }

    public void LoadScene(string sceneName) => StartCoroutine(AsyncLoadingScreen(sceneName));

    IEnumerator AsyncLoadingScreen(string sceneName)
    {
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);

        screen.SetActive(true);
        while (!loading.isDone)
        {
            float progress = Mathf.Clamp01(loading.progress / 0.9f);
            loadingFillAmount.fillAmount = progress;
        }
        screen.SetActive(false);
        yield return null;
    }
}
