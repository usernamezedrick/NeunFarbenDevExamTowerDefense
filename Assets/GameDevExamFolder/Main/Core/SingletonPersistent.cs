using UnityEngine;
using UniRx;

namespace NF.Main.Core
{
    public abstract class SingletonPersistent<T> : MonoExt where T : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(gameObject);
                Initialize(); // Calls parameterless Initialize (or override Initialize(object) if needed)
                OnSubscriptionSet();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Optional override if you wish to pass initialization data.
        public virtual void Initialize(object data = null)
        {
            Initialize();
        }
    }
}
