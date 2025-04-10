namespace Main.Singleton
{
    /// <summary>
    /// The singleton implementation for classes.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Singleton<T> where T : Singleton<T>, new()
    {

        #region Fields

        /// <summary>
        /// The instance.
        /// </summary>
        private static T instance;
        #endregion

        #region Properties

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
                    lock (typeof(T))
                    {
                        if (instance == null)
                        {
                            instance = new T();
                        }
                    }
                }

                return instance;
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

            instance.OnSingletonClear();
            instance = default(T);
        }

        #endregion

    }

}