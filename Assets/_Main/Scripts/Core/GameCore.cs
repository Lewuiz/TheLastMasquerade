using Main.Singleton;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class GameCore : PersistentMonoSingleton<GameCore>
    {
        [SerializeField] private StoryManager storyManager = default;
        public SaveManager SaveManager { get; private set; } = default;

        public IEnumerator Initialized()
        {
            yield return null;
            // Initialize in order of priority
            InitializedSaveData();
            InitializedStoryManager();

            Debug.Log($"[{nameof(GameCore)}] has been initialized successfully");
        }

        private void InitializedSaveData()
        {
            SaveManager = new SaveManager(); //TODO: should i use it as monobehaviour ?
        }

        private void InitializedStoryManager()
        {
            storyManager.Init();
        }
    }
}
