using UnityEngine;

namespace Main.Singleton
{
    /// <summary>
    /// The basic MonoBehaviour singleton implementation, this singleton is destroyed after scene changes, use <see cref="PersistentMonoSingleton{T}"/> if you want a persistent and global singleton instance.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// The instance.
        /// </summary>
        private static T instance;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Use this for initialization.
        /// </summary>
        protected virtual void Awake()
        {
            if (!Application.isPlaying)
                return;

            if (instance == null)
            {
                instance = this as T;
            }
            else
            {

                // Destory duplicates
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }
            }
        }

        public virtual void OnSingletonClear() { }

        public static void CreateInstance()
        {
            DestroyInstance();
            instance = Instance;
        }

        public static void DestroyInstance()
        {
            if (instance == null)
            {
                return;
            }

            MonoSingleton<T> monoSingleton = instance as MonoSingleton<T>;
            monoSingleton.OnSingletonClear();
            instance = default(T);
        }

        private void OnDestroy()
        {
            DestroyInstance();
        }
    }
}