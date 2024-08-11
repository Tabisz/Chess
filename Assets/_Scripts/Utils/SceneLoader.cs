using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum SceneEnum
{
    MENU,
    GAMEPLAY
}

public enum SceneType
{
    OTHER,
    MENU,
    GAMEPLAY,
}

[Serializable]
public class ScenePackage
{
    public SceneEnum MyEnum;
    [SerializeField]
    private string myName;
    public string MyName => myName;

}


public class SceneLoader: MonoBehaviour
{
    private Action OnLoadStarted;
    private Action OnLoadEnded;

    public List<ScenePackage> GameScenes;

    public ScenePackage CurrentScene { get; private set; }


    public void Init()
    {
        var sceneName = SceneManager.GetActiveScene().name;
        CurrentScene = GetScene(sceneName);
    }
    public void LoadScene(SceneEnum sceneEnum, Action onBeforeLoad,Action OnAfterLoad)
    {
        var sceneName = GetScene(sceneEnum);
        if (sceneName == string.Empty)
        {
            Debug.Log("Scene you try to load is unknown");
            return;
        }

        OnLoadStarted += onBeforeLoad;
        OnLoadEnded += OnAfterLoad;
        StartCoroutine(LoadYourAsyncScene(sceneName));
    }


    IEnumerator LoadYourAsyncScene(string sceneName)
    {
        OnLoadStarted?.Invoke();
        OnLoadStarted = null;
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        OnLoadEnded?.Invoke();
        OnLoadEnded = null;
    }

    private string GetScene(SceneEnum sceneEnum)
    {
        if (GameScenes == null) return string.Empty;
        var scenePackage = GameScenes.Find(x => x.MyEnum == sceneEnum);
        if (scenePackage != null)
            return scenePackage.MyName;
        return string.Empty;
    }

    private ScenePackage GetScene(string sceneName)
    {
        if (GameScenes == null) return null;
        var package = GameScenes.Find(x => x.MyName == sceneName);
        if (package != null)
            return package;
        return null;
    }

}
