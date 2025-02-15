namespace NF.Main.Core
{
    public abstract class BaseState : IState
    {
        // Constructor to call by derived classes
        protected BaseState()
        {
        }

        public virtual void OnEnter()
        {
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public virtual void OnExit()
        {
        }
    }
}