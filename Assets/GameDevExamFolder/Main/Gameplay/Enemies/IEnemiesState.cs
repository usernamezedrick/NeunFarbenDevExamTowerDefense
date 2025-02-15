namespace NF.Main.Gameplay.Enemies
{
    public interface IEnemyState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void OnExit();
    }
}
