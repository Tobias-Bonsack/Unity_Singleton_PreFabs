using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneLoader
{
    public class Manager : MonoBehaviour
    {
        [SerializeField] SceneLoader.EventSystem _eventS;

        [Header("Transition-Objects")]
        [SerializeField] GameObject _transition;
        [SerializeField] string _triggerText;
        [SerializeField] float _startTimeScale = 1f, _endTimeScale = 0f;

        private void Awake()
        {
            _eventS._onEndScene += ActionOnEndScene;
            _eventS._onStartScene += ActionOnStartScene;
        }

        #region event observer
        private void ActionOnEndScene(object sender, SceneLoader.EventSystem.OnEndSceneEventArgs args)
        {
            Time.timeScale = _endTimeScale;
            _transition.GetComponent<SceneLoader.AnimationEventObserver>()._sceneNumber = args._sceneNumber;
            _transition.GetComponent<Animator>().SetTrigger(_triggerText);
        }

        private void ActionOnStartScene(object sender, EventArgs args) => Time.timeScale = _startTimeScale;

        #endregion
    }
}
