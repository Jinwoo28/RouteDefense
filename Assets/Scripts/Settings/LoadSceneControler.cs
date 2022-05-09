using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class LoadSceneControler : MonoBehaviour
{
    static string nextScene;

    [SerializeField] private TextMeshProUGUI prograssbar = null;

    public static void LoadScene(string _nextScene)
    {
        nextScene = _nextScene;
        SceneManager.LoadScene("LoadScene");
    }

    private void Start()
    {
        StartCoroutine(LoadProcess());
    }

    IEnumerator LoadProcess()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

                prograssbar.text = "Loading progress: " + (operation.progress * 100) + "%";
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

        }
    }


    public IEnumerator DDDD()
    {
        var sceneLoader = SceneManager.LoadSceneAsync(nextScene, LoadSceneMode.Additive);

        float elapsed = 0.0f;

        // The amount of time to spread the loading progress over.
        // If the actual time is longer than this, then it will sit at maxSceneLoadProgress until it's actually finished.
        // If shorter, then it will jump to maxSceneLoadProgress when finished.
        // This results in a smoother scene loading progress meter than using sceneLoader.progress, which jumps quickly to 0.9 then sits on that for a while before actually finishing.
        const float MAX_ELAPSE = 2.0f;

        do
        {
            prograssbar.text = (Mathf.Clamp01(elapsed / MAX_ELAPSE) * 100).ToString();
            yield return null;
            elapsed += Time.deltaTime;
        } while (!sceneLoader.isDone);

        //setProgressMeter(maxSceneLoadProgress);
    }



}
