using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    public GameObject enemyInfo;

    // Start is called before the first frame update
    void Start()
    {
        //this.enemyInfo.SetActive(false);
        this.enemyInfo.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
