namespace CombatManager
{
    using Assets.Scripts;

    using DG.Tweening;

    using UnityEngine;

    public class BattleController : MonoBehaviour
    {
        public static GameStates GameState { get; private set; } = GameStates.Null;


        /// <summary>
        ///     The main entry function call to start the battle.
        /// </summary>
        public void ShouldStartBattle()
        {
            if (GameState != GameStates.Null)
            {
                Debug.LogWarning("Attempt To Start a battle when battle is already started.");

                return;
            }

            // Changing state
            GameState = GameStates.Entering;

            // this.timer.StartLevel();
            
        }

        protected void OnEnable()
        {
            DOTween.Init(true, true);
        }
    }
}