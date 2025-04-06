using Main.Singleton;
using System.Collections;
using UnityEngine;

namespace Main
{
    public class GameCore : PersistentMonoSingleton<GameCore>
    {
        public IEnumerator Initialized()
        {
            yield return null;
        }
    }
}
