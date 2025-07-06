using DG.Tweening;
using System.Collections;
using UnityEngine;
using Main.Scene;

namespace Main
{
    public abstract class RootSceneController : SceneController
    {
        private bool hasInitializedGameModule = false;
        protected override IEnumerator InitializeGameModule()
        {
            if (hasInitializedGameModule)
                yield break;

            hasInitializedGameModule = true;

            DOTween.Init();
            DOTween.SetTweensCapacity(200, 200);

            GameObject windowController = Resources.Load<GameObject>("Core/Window Controller");
            Instantiate(windowController);
            WindowController.Instance.Init();

            GameObject gameCoreGO = Resources.Load<GameObject>("Core/Game Core");
            Instantiate(gameCoreGO);
            yield return GameCore.Instance.Initialized();
        }
    }
}
