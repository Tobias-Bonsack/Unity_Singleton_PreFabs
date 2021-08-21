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
        public int _sceneNumber;
        public void EndObserver()
        {
            StartCoroutine(LoadAsyncScene());
        }

        private IEnumerator LoadAsyncScene()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneNumber);

            while (asyncOperation.progress < 1f)
            {
                Debug.Log(asyncOperation.progress);
                _loadBar.fillAmount = asyncOperation.progress;
                yield return new WaitForEndOfFrame();
            }

        }

        public void StartObserver()
        {
            _eventS.TriggerOnStartScene();
        }
    }
}
