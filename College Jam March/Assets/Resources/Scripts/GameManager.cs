using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public static GameManager instance;
    public GameObject loadScreen;

    private void Awake()
    {
        instance = this;

        SceneManager.LoadSceneAsync((int)GameScene.MAINMENU, LoadSceneMode.Additive);
    }

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public void LoadAlpha()
    {
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)GameScene.MAINMENU));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)GameScene.PLAYERSCENE, LoadSceneMode.Additive));
        //In the future take the game save and load to that scene with that position
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)GameScene.ALPHATUTORIAL, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadMainMenu(Scene unloadScene)
    {
        scenesLoading.Add(SceneManager.UnloadSceneAsync(unloadScene));
        scenesLoading.Add(SceneManager.UnloadSceneAsync((int)GameScene.PLAYERSCENE));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)GameScene.MAINMENU, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    public void LoadLevel(int sceneIndex, Scene unloadScene)
    {
        scenesLoading.Add(SceneManager.UnloadSceneAsync(unloadScene));
        scenesLoading.Add(SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive));
        StartCoroutine(GetSceneLoadProgress());
    }

    public void ExitGame()
    {
        //Unload scenes and quit
        Application.Quit();
    }

    public IEnumerator GetSceneLoadProgress()
    {
        loadScreen.SetActive(true);
        loadScreen.LeanAlpha(0, 0);
        loadScreen.LeanAlpha(256, 1f);

        for (int i=0; i < scenesLoading.Count; i++)
        {
            while (!scenesLoading[i].isDone)
            {
                yield return null;
            }
        }

        loadScreen.LeanAlpha(0, 1f);
        yield return new WaitForSeconds(1);
        loadScreen.SetActive(false);
    } 
}