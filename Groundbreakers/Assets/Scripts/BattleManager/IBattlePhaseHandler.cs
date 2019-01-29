namespace Assets.Scripts
{
    public interface IBattlePhaseHandler
    {
        #region Messaging System Functions

        void OnTilesEntering();

        void OnBattling();

        void OnTilesExiting();

        #endregion
    }
}