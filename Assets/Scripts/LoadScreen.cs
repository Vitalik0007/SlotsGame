using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [SerializeField] private Slider scale;

    private void Start()
    {
        Loading();
    }

    private void Loading()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(1);
        //loadAsync.allowSceneActivation = false;

        while(!loadAsync.isDone)
        {
            float progressValue = Mathf.Clamp01(loadAsync.progress / 0.9f);
            scale.value = progressValue;

            /*if (loadAsync.progress >= .9f && !loadAsync.allowSceneActivation)
            {
                yield return new WaitForSeconds(2.2f);
                loadAsync.allowSceneActivation = true;
            }*/
            yield return null;
        }
    }
}
