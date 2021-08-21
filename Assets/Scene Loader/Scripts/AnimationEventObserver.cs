using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneLoader
{
    public class AnimationEventObserver : MonoBehaviour
    {
        [SerializeField] SceneLoader.EventSystem _eventS;
        [SerializeField] Image _loadBar;
        [SerializeField] float _WaitAfterLoaded = 1f;
        public int _sceneNumber;
        public void EndObserver()
        {
            StartCoroutine(LoadAsyncScene());
        }

        private IEnumerator LoadAsyncScene()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneNumber);
            asyncOperation.allowSceneActivation = false;

            while (asyncOperation.progress < 0.9f)
            {
                Debug.Log(asyncOperation.progress);
                yield return new WaitForEndOfFrame();
            }
            _loadBar.fillAmount = asyncOperation.progress;

            yield return new WaitForSecondsRealtime(_WaitAfterLoaded);
            asyncOperation.allowSceneActivation = true;

        }

        public void StartObserver()
        {
            _eventS.TriggerOnStartScene();
        }
    }
}
