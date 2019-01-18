using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    public GameObject standardCharacterPrefab;
    private GameObject characterToPlace;

    public static int characterCount = 0;

    public static int maximumCharacter = 5;





    void Awake()
    {
        if (instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }

    }

    void Start()
    {
        this.characterToPlace = this.standardCharacterPrefab;

    }


    public GameObject GetCharacterToPlace()
    {
        return this.characterToPlace;
    }

}
