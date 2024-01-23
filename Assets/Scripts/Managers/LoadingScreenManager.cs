using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class LoadingScreenManager : MonoBehaviour
{
    static LoadingScreenManager instance;
    public static LoadingScreenManager Instance { get { return instance; } }

    [SerializeField] GameObject screen;
    [SerializeField] Slider loadingFillAmount;
    [SerializeField] TextMeshProUGUI tooltip;
    [SerializeField] System.Collections.Generic.List<string> tips = new System.Collections.Generic.List<string>();

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
    public void LoadSceneWithMove(string sceneName, string key)
    {
        GameManager.Instance.ChangePlayerPos(TransitionSpawns.ReturnSpawn(key));
        GameManager.Instance.activeScenes.Clear();
        GameManager.Instance.activeScenes.Add(sceneName);
        StartCoroutine(AsyncLoadingScreen(sceneName));
        GameManager.Instance._player.GetComponent<PlayerMovement>().UpdateAgent(TransitionSpawns.ReturnSpawn(key));
    }
    IEnumerator AsyncLoadingScreen(string sceneName)
    {
        StopCoroutine(ChangeTipText());
        loadingFillAmount.value = 0;
        screen.SetActive(true);
        StartCoroutine(ChangeTipText());
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        while (!loading.isDone)
        {
            float progress = Mathf.Clamp01(loading.progress / 0.9f);
            loadingFillAmount.value = progress;
            yield return null;
        }
        screen.SetActive(false);
    }

    IEnumerator ChangeTipText()
    {
        while (true)
        {
            tooltip.text = ChangeText();
            StartCoroutine(Fade(true, tooltip));
            yield return new WaitForSeconds(5f);
            StopCoroutine(Fade(true, tooltip));
            yield return StartCoroutine(Fade(false, tooltip));
            yield return new WaitForSeconds(1f);
            StopCoroutine(Fade(false, tooltip));
        }
    }

    int lastIndex = 0;

    string ChangeText()
    {
        int tipIndex;
        do
        {
            tipIndex = Random.Range(0, tips.Count);
        } while (tipIndex == lastIndex);
        lastIndex = tipIndex;
        return tips[tipIndex].ToString();
    }

    IEnumerator Fade(bool type, TextMeshProUGUI txt)
    {
        if (type) txt.DOFade(1, 1f);
        else txt.DOFade(0, 1f);
        yield return null;
    }
}
