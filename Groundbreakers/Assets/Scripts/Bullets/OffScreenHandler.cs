namespace Assets.Scripts
{
    using UnityEngine;

    /// <summary>
    ///     A simple component that automatically kills the GameObject that goes out of screen.
    /// </summary>
    public class OffScreenHandler : MonoBehaviour
    {
        /// <summary>
        ///     Using this native structure to determine if the bullet should be killed.
        /// </summary>
        private static readonly Bounds ValidBounds = new Bounds(new Vector3(4.0f, 4.0f), new Vector3(10.0f, 10.0f));

        private static bool CheckOutOfBounds(Vector3 position)
        {
            var xy = new Vector3(position.x, position.y, 0.0f);

            return !ValidBounds.Contains(xy);
        }

        private void Update()
        {
            if (CheckOutOfBounds(this.transform.position))
            {
                // should trigger some event here, if we want dangerous factors later in the game.
                Destroy(this.gameObject);
            }
        }
    }
}