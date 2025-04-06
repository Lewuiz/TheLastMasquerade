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

            GameCore gameCore = Resources.Load<GameCore>("Core/Game Core");
            Instantiate(gameCore.gameObject);
            yield return gameCore.Initialized();
        }
    }
}
