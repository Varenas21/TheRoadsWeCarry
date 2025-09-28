using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject loadingScreen;

    private string sceneToLoad;

    public enum SceneName
    {
        MainMenu,
        MainGame,
        TestScene
    }

    public void Start()
    {
        if (m_Animator == null) { m_Animator = GetComponent<Animator>(); }
        if(loadingScreen != null) { loadingScreen.SetActive(false); }
    }
    public void LoadSceneByName(string sceneName)
    {
        Debug.Log("Scene Loading");
        sceneToLoad = sceneName;
        m_Animator.SetTrigger("FadeOut");
    }

    public void OnFadeOutComplete()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        m_Animator.SetTrigger("FadeIn");
    }

    public void OnFadeInComplete()
    {
        if (loadingScreen != null) { loadingScreen.SetActive(false); }
    }

    public void LoadSceneByIndex(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void UnloadScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(sceneName);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }
}
