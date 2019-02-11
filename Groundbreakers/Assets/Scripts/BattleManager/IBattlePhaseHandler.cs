namespace Assets.Scripts
{
    /// <summary>
    /// This is an interface class that provide some Battle phase utility functions.
    /// Attach this interface to classes that you think will involves in different battle phase.
    /// </summary>
    public interface IBattlePhaseHandler
    {
        #region Messaging System Functions

        /// <summary>
        /// This function is automatically triggered when the battle begin. 
        /// </summary>
        void OnBattleBegin();

        /// <summary>
        /// This function is automatically triggered when the battle terminates. 
        /// </summary>
        void OnBattleEnd();

        #endregion
    }
}