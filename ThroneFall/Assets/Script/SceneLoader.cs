using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }    }

    public void Start()
    {

    }

    public void StartLoadSceneAsync(string sceneName, Action onLoaded = null, bool unloadCurrent = false)
    {
        StartCoroutine(LoadSceneAsync(sceneName, onLoaded, unloadCurrent));
    }
    public IEnumerator LoadSceneAsync(string sceneName, Action onLoaded = null, bool unloadCurrent = false)
    {
        var loadLoading = SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        while (!loadLoading.isDone)
            yield return null;

        Scene currentScene = SceneManager.GetActiveScene();

        if (unloadCurrent)
        {
            AsyncOperation unloadOp = SceneManager.UnloadSceneAsync(currentScene);
            while (!unloadOp.isDone)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(2f);
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!loadOp.isDone)
        {
            yield return null;
        }
        var unLoadLoadingSceneOp =  SceneManager.UnloadSceneAsync("LoadingScene");
        while (!unLoadLoadingSceneOp.isDone)
        {
            yield return null;
        }
        onLoaded?.Invoke();

    }

    public void LoadSingleScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}