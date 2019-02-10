namespace Assets.Scripts
{
    using UnityEngine;

    /// <summary>
    /// This component handles tile enter/exiting animation. Attaching this component to objects
    /// that you wish to falling and fade.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnterBehavior : MonoBehaviour
    {
        #region Inspector Fields

        [SerializeField]
        private Vector2 offset;

        #endregion

        #region Internal fields



        #endregion

        #region Public Funcitons



        #endregion

        #region Unity Callbacks

        private void Start()
        {

        }

        private void FixedUpdate()
        {
            
        }

        #endregion

        #region Internal Functions



        #endregion
    }
}