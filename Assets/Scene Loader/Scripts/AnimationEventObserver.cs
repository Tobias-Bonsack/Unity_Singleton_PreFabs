using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoader
{
    public class AnimationEventObserver : MonoBehaviour
    {
        [SerializeField] SceneLoader.EventSystem _eventS;
        public int _sceneNumber;
        public void EndObserver()
        {
            SceneManager.LoadScene(_sceneNumber);
        }

        public void StartObserver()
        {
            _eventS.TriggerOnStartScene();
        }
    }
}
