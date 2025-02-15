using UnityEngine;
using UniRx;
using System;

namespace NF.Main.Core
{
    public abstract class MonoExt : MonoBehaviour
    {
        public virtual void Initialize() { }
        public virtual void OnSubscriptionSet() { }

        protected void AddEvent<T>(IObservable<T> observable, System.Action<T> action)
        {
            observable.Subscribe(action).AddTo(this);
        }
    }
}
