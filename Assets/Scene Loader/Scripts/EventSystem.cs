using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SceneLoader
{
    public class EventSystem : MonoBehaviour
    {
        #region events
        public event EventHandler<OnEndSceneEventArgs> _onEndScene;
        public event EventHandler<EventArgs> _onStartScene;

        #endregion

        #region event args
        public class OnEndSceneEventArgs
        {
            public int _sceneNumber;
        }

        #endregion

        #region triggers
        public void TriggerOnEndScene(int sceneNumber) => _onEndScene?.Invoke(null, new OnEndSceneEventArgs { _sceneNumber = sceneNumber });
        public void TriggerOnStartScene() => _onStartScene?.Invoke(null, EventArgs.Empty);

        #endregion
    }
}
