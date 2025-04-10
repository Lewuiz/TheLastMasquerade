using Main.Singleton;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class GameCore : PersistentMonoSingleton<GameCore>
    {
        public SaveManager SaveManager { get; private set; } = default;

        public IEnumerator Initialized()
        {
            yield return null;
            InitializedSaveData();

            Debug.Log($"[{nameof(GameCore)}] has been initialized successfully");
        }

        private void InitializedSaveData()
        {
            SaveManager = new SaveManager(); //TODO: should i use it as monobehaviour ?
        }
    }
}
