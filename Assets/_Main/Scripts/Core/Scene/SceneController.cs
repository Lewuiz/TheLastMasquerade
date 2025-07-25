using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Scene
{
    public abstract class SceneController : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return InitializeGameModule();
            UpdateWindowCanvasCamera();
            OnStartCompleted();
        }

        protected abstract IEnumerator InitializeGameModule();

        protected virtual void OnStartCompleted()
        {

        }

        protected abstract void UpdateWindowCanvasCamera();

        protected void LoadScene(SceneID sceneId)
        {
            StartCoroutine(LoadSceneCor((int)sceneId));
        }

        private IEnumerator LoadSceneCor(int sceneIdx)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneIdx);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                Debug.Log($"Loading scene progress: {asyncLoad.progress * 100}%");
                if (asyncLoad.progress >= 0.9f)
                {
                    Debug.Log("Scene is Loaded.");
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}
