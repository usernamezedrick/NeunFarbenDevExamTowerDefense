using UnityEngine;

namespace NF.Main.Core
{
    public class SingletonPersistent<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public virtual void Initialize(object data = null)
        {
            Debug.Log(typeof(T).Name + " initialized.");
        }
    }
}
