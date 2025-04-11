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
;
            GameObject gameCoreGO = Resources.Load<GameObject>("Core/Game Core");
            Instantiate(gameCoreGO);
            yield return GameCore.Instance.Initialized();
        }
    }
}
