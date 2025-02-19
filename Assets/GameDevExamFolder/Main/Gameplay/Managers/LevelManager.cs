using UnityEngine;

namespace NF.Main.Gameplay.Managers
{
    /// <summary>
    /// A stub LevelManager that handles enemy destruction events.
    /// </summary>
    public class LevelManager : MonoBehaviour
    {
        /// <summary>
        /// Called when an enemy is destroyed.
        /// </summary>
        public void EnemyDestroyed()
        {
            Debug.Log("Enemy destroyed. (Notified by LevelManager)");
           
        }
    }
}
