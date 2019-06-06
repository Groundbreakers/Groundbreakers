namespace TileMaps
{
    using System.Linq;

    using Core;

    using UnityEngine;
    using UnityEngine.Assertions;

    public class RetreatCharacterOnDestoried : MonoBehaviour
    {
        private PartyManager party;

        private void OnEnable()
        {
            this.party = GameObject.FindObjectOfType<PartyManager>();

            Assert.IsNotNull(this.party);
        }

        private void OnDisable()
        {
            var characters = this.party.GetDeployedCharacters();

            var pos = this.transform.position;

            var target = characters.Where(
                x => Vector3.Distance(pos, x.transform.position) <= Mathf.Epsilon)
                .ToList();

            if (target.Any())
            {
                this.party.RetrieveCharacter(target.First());
            }
        }
    }
}