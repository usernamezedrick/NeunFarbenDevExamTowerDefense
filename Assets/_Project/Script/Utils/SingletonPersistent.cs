using UnityEngine;

namespace NF.Main.Core
{
	/// <summary>
	/// Be aware this will not prevent a non singleton constructor
	///   such as `T myT = new T();`
	/// To prevent that, add `protected T () {}` to your singleton class.
	/// 
	/// As a note, this is made as MonoBehaviour because we need Coroutines.
	/// </summary>
	///
	public class SingletonPersistent<T> : MonoExt where T : MonoExt
	{
		protected static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					var objs = FindObjectsOfType(typeof(T)) as T[];
					if (objs.Length > 0)
						instance = objs[0];
					if (objs.Length > 1)
					{
						Debug.LogError("There is more than one " + typeof(T).Name + " in the scene.");
					}
				}

				return instance;
			}

			set { instance = value; }
		}

		private void OnEnable()
		{
			if (instance != this && instance != null)
			{
				Destroy(gameObject);
				return;
			}
			DontDestroyOnLoad(this.gameObject);
		}

		public void UnsetSingleton()
		{
			Destroy(Instance);
			Instance = null;
		}

		/// <summary>
		/// When Unity quits, it destroys objects in a random order.
		/// In principle, a Singleton is only destroyed when application quits.
		/// If any script calls Instance after it have been destroyed, 
		///   it will create a buggy ghost object that will stay on the Editor scene
		///   even after stopping playing the Application. Really bad!
		/// So, this was made to be sure we're not creating that buggy ghost object.
		/// </summary>
		public void OnDestroy()
		{
			if (instance != null)
			{
				instance = null;
			}
		}
	}
}
