using UnityEngine;

namespace Main.Singleton
{
    /// <summary>
    /// This singleton is persistent across scenes by calling <see cref="UnityEngine.Object.DontDestroyOnLoad(Object)"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PersistentMonoSingleton<T> : MonoBehaviour where T : Component
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
                DontDestroyOnLoad(instance);
            }
            else
            {
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

        protected virtual void OnSingletonClear() { }

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

            PersistentMonoSingleton<T> persistentMonoSingleton = instance as PersistentMonoSingleton<T>;
            persistentMonoSingleton.OnSingletonClear();
            instance = default(T);
        }
    }
}