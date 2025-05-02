using Main.Singleton;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class GameCore : PersistentMonoSingleton<GameCore>
    {
        [field: SerializeField] public StoryManager StoryManager { get; private set; } = default;
        public SaveManager SaveManager { get; private set; } = default;
        public StorySceneData StorySceneData { get; private set; } = default;

        private bool hasInitialized = false;

        public IEnumerator Initialized()
        {
            if (hasInitialized)
                yield break;

            yield return null;
            // Initialize in order of priority
            InitializedSaveData();
            InitializedStoryManager();
            InitializedOverlay();
            InitializeStorySceneData();

            hasInitialized = true;
            Debug.Log($"[{nameof(GameCore)}] has been initialized successfully");
        }

        private void InitializedSaveData()
        {
            SaveManager = new SaveManager(); //TODO: should i use it as monobehaviour ?
        }

        private void InitializedStoryManager()
        {
            StoryManager.Init();
        }

        private void InitializeStorySceneData()
        {
            StorySceneData = new StorySceneData();
        }

        private void InitializedOverlay()
        {
            GameObject overlayGO =  Resources.Load<GameObject>("Core/Overlay Canvas");
            Instantiate(overlayGO);
            Overlay.Instance.Init();
        }
    }
}
