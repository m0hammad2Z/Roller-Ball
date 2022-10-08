using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
   public GameObject loadingScreen;
   public bool loadingScene;
   public Slider slider;

   void Start()
   {
    if(loadingScene)
    LoadLevel(1);
   }

   public void LoadLevel(int sceneIndex)
   {
    if(!loadingScene)
    loadingScreen.SetActive(true);
    StartCoroutine(LoadAsynch(sceneIndex));
   }

   IEnumerator LoadAsynch(int sceneIndex)
   {
    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
    

    while(!operation.isDone)
    {
        float progress = Mathf.Clamp01(operation.progress / 0.9f);
        slider.value = progress;
        yield return null;
    }
   }
}
