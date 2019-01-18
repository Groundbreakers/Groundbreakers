// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BuildManager.cs" company="UCSC">
//   MIT
// </copyright>
// <summary>
//   Javy Wu
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static int characterCount = 0;

    public static BuildManager instance;

    public static int maximumCharacter = 5;

    public GameObject standardCharacterPrefab;

    private GameObject characterToPlace;

    public GameObject GetCharacterToPlace() {
        return this.characterToPlace;
    }

    void Awake() {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
    }

    void Start() {
        this.characterToPlace = this.standardCharacterPrefab;
    }
}
