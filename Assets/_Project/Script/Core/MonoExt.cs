using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UniRx;  

namespace NF.Main.Core
{
    //This class is an extension of the monobehaviour class
    //The goal of this class is to automatically handle garbage collection
    //And to organize the way data is being initialized, and handle adding of subscription events
    public class MonoExt : SerializedMonoBehaviour
    {
        //This is the list we dispose on disabling or destroying of the game object, this is use for garbage collection
        protected List<IDisposable> _disposables = null;

        public virtual void Initialize(object data = null)
        {
            _disposables = new List<IDisposable>();
        }

        public virtual void Dispose()
        {
            _disposables?.ForEach(p => p?.Dispose());
            _disposables?.Clear();
        }

        public virtual void OnSubscriptionSet()
        {

        }

        protected virtual void AddEvent<T>(Subject<T> subject, Action<T> action)
        {
            subject.Subscribe(action).AddTo(_disposables);
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
