using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cetus_Script : MonoBehaviour
{
    // Stage flags
    private bool entering = true;
    private bool combat = false;
    private bool dying = false;

    // Timers
    private float stunTime = 0;
    private float blightTimer = 1;
    private float burnTimer = 1;

    private float cleanseTimer = 12;
    private float waterStrikeTimer;
    private float splashTimer;
    private float chargeShotTimer;

    private float entranceTimer = 4;

    // Sorting layer groundtiles 4.5

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Entrance());
    }

    // Update is called once per frame
    void Update()
    {
        // Only do combat things if the fight has begun
        if (this.combat == true)
        {

        }
    }

    // Entrance animation
    private IEnumerator Entrance()
    {
        // (3.5, 2) -> (3.5, 3.5)
        this.transform.position = new Vector3(3.5f,1f,0f);
        yield return new WaitForSeconds(10);
        for (float i = entranceTimer; i >= 0; i -= Time.deltaTime)
        {
            transform.Translate(Vector3.up / 90, Space.World);
            transform.Rotate(Vector3.forward, 720 * Time.deltaTime);
            yield return null;
        }
        this.transform.rotation = Quaternion.identity;
        yield return new WaitForSeconds(1);
        startCombat();
    }

    // Start combat
    private void startCombat()
    {
        this.combat = true;
        this.tag = "Enemy";
    }
}
