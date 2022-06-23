using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Camera mainCamera;
    public string sceneToLoad;
    public CanvasGroup canvasGroup;
    public Text percentLoadedText;
    public Image percentLoadedImg;

    private AsyncOperation loadingOperation;

    float currentProgress = 0;
    float delta = 0;

    void Awake()
    {
        SetProgress(0);
    }

    void Start()
    {
        StartLoad();
    }

    void SetProgress(float progressValue)
    {
        if (percentLoadedText != null) percentLoadedText.text = "Loading (" + Mathf.Round(progressValue * 100) + "%)";
        percentLoadedImg.fillAmount = progressValue;
        if (loadingOperation != null && progressValue == 1)
        {
            HideLoading();
        }
    }
    IEnumerator LoadSceneAsyncProcess(string sceneName)
    {
        loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        loadingOperation.allowSceneActivation = false;
        while (currentProgress < 1)
        {
            delta = (loadingOperation.progress + 0.1f - currentProgress) * Time.deltaTime;
            delta = Math.Clamp(delta, 0, 0.01F);
            currentProgress += delta;
            if (currentProgress >= 0.99) currentProgress = 1;
            SetProgress(currentProgress);
            yield return new WaitForEndOfFrame();
        }
    }

    void StartLoad()
    {
        StartCoroutine(LoadSceneAsyncProcess(sceneToLoad));
    }

    void FadeLoadingScreen(float targetValue, float duration, Action callback = null)
    {
        canvasGroup.DOFade(targetValue, duration).SetEase(Ease.Linear).OnComplete(() => callback?.Invoke());
    }

    void HideLoading()
    {
        loadingOperation.allowSceneActivation = true;
        Destroy(mainCamera.gameObject);
        FadeLoadingScreen(0, 0.5f, () =>
        {
            Destroy(canvasGroup.gameObject);
        });
    }
}
