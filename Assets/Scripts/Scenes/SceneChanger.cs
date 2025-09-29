using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class SceneChanger : MonoBehaviour
{
    enum FadeStatus
    {
        FADING_IN,
        FADING_OUT,
        NONE
    }

    public static SceneChanger instance;
    public static SceneLoader loader;

    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration;

    private FadeStatus currentStatus = FadeStatus.NONE;
    private float fadeTime;
    private string sceneToLoad;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            loader = FindFirstObjectByType<SceneLoader>();
        }
        else
        {
            Destroy(gameObject);
        }

        fadeImage.color = Color.black;
        currentStatus = FadeStatus.FADING_IN;
        fadeTime = 0;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        fadeImage.color = Color.black;
        currentStatus = FadeStatus.FADING_IN;
        fadeTime = 0;
    }

    public void ChangeScene(string _SceneName)
    {
        sceneToLoad = _SceneName;
        currentStatus = FadeStatus.FADING_OUT;
        fadeTime = 0;
    }

    private void Update()
    {
        if (currentStatus == FadeStatus.NONE) { return; }

        fadeTime += Time.deltaTime;

        if (fadeTime > fadeDuration)
        {
            fadeTime = fadeDuration;
        }

        float alpha = 0;

        if (currentStatus == FadeStatus.FADING_OUT)
        {
            alpha = Mathf.Lerp(0, 1, fadeTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);

            if (fadeTime >= fadeDuration)
            {
                loader.LoadSceneByName(sceneToLoad);
                currentStatus = FadeStatus.NONE;
            }
        }
        else if (currentStatus == FadeStatus.FADING_IN)
        {
            alpha = Mathf.Lerp(1, 0, fadeTime / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);

            if (fadeTime >= fadeDuration)
            {
                currentStatus = FadeStatus.NONE;
            }

        }
    }
}
