using DG.Tweening;
using System.Collections;
using UnityEngine;
using Main.Scene;

namespace Main
{
    public class RootSceneController : SceneController
    {
        private bool hasInitializedGameModule = false;
        protected override IEnumerator InitializeGameModule()
        {
            if (hasInitializedGameModule)
                yield break;

            hasInitializedGameModule = true;

            DOTween.Init();
            DOTween.SetTweensCapacity(200, 200);

            GameObject gameObject = new GameObject();
            GameCore gameCore = gameObject.AddComponent<GameCore>();
            yield return gameCore.Initialized();
        }
    }
}
