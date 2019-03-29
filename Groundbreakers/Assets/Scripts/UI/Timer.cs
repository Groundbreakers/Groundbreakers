namespace Assets.Scripts
{
    using UnityEngine;
    using UnityEngine.UI;

    public class Timer : MonoBehaviour, IBattlePhaseHandler
    {
        #region Inspector values

        [SerializeField]
        private GameObject ui;

        [SerializeField]
        private Text wave;

        [SerializeField]
        private Text timer;

        #endregion

        #region Internal Fields

        private float countdown;
        private float waveDelay;
        private int waveCount;

        #endregion

        #region IBattlePhaseHandler

        public void OnBattleBegin()
        {
            this.ResetTimer();
            this.ui.SetActive(true);
        }

        public void OnBattleEnd()
        {
            this.ui.SetActive(false);
        }

        public void OnBattleVictory()
        {
        }

        #endregion

        #region Unity Callbacks

        private void OnEnable()
        {
            BattleManager.StartListening("start", this.OnBattleBegin);
            BattleManager.StartListening("end", this.OnBattleEnd);
        }

        // Update is called once per frame
        private void Update()
        {
            if (BattleManager.GameState != GameStates.Null)
            {
                if (this.countdown <= 0F)
                {
                    this.waveCount += 1;
                    this.wave.text = "WAVE " + this.waveCount + "/5";
                    this.countdown = this.waveDelay;
                }

                this.countdown -= Time.deltaTime;
                this.timer.text = Mathf.Round(this.countdown).ToString();
            }
        }

        #endregion

        #region Internal Functions

        private void ResetTimer()
        {
            this.countdown = 10.0F;
            this.waveDelay = 30.0F;
            this.waveCount = 0;
            this.wave.text = "BEGINS IN";
            this.timer.text = this.countdown.ToString();
        }

        #endregion
    }
}
