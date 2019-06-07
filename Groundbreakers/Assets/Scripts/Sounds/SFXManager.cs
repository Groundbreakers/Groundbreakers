using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip CharacterBladeHit;
    public AudioClip CharacterBulletHit;
    public AudioClip CharacterShoot;
    public AudioClip CharacterSwing;
    public AudioClip CharacterTransform;
    public AudioClip EnemyDie;
    public AudioClip MenuClick;
    public AudioClip PlayerDeploy;
    public AudioClip PlayerHpLoss;
    public AudioClip TileDeploy;
    public AudioClip TileError;
    public AudioClip TimerCountdown;
    public AudioClip Victory;

    private AudioSource SFX;

    void Start()
    {
        this.SFX = this.GetComponent<AudioSource>();
    }

    /// <summary>
    ///     The play sfx.
    /// </summary>
    /// <example>
    ///     GameObject.Find("SFX Manager").GetComponent<SFXManager>().PlaySFX("CharacterShoot");
    /// </example>
    /// <param name="title">
    ///     The title.
    /// </param>
    public void PlaySFX(string title)
    {
        switch (title)
        {
            case "CharacterBladeHit":
                this.SFX.PlayOneShot(this.CharacterBladeHit);
                break;

            case "CharacterBulletHit":
                this.SFX.PlayOneShot(this.CharacterBulletHit, .5f);
                break;

            case "CharacterShoot":
                this.SFX.PlayOneShot(this.CharacterShoot, .5f);
                break;

            case "CharacterSwing":
                this.SFX.PlayOneShot(this.CharacterSwing);
                break;

            case "CharacterTransform":
                this.SFX.PlayOneShot(this.CharacterTransform);
                break;

            case "EnemyDie":
                this.SFX.PlayOneShot(this.EnemyDie, .5f);
                break;

            case "MenuClick":
                this.SFX.PlayOneShot(this.MenuClick);
                break;

            case "PlayerDeploy":
                this.SFX.PlayOneShot(this.PlayerDeploy);
                break;

            case "PlayerHpLoss":
                this.SFX.PlayOneShot(this.PlayerHpLoss);
                break;

            case "TileDeploy":
                this.SFX.PlayOneShot(this.TileDeploy);
                break;

            case "TileError":
                this.SFX.PlayOneShot(this.TileError);
                break;

            case "TimerCountdown":
                this.SFX.PlayOneShot(this.TimerCountdown);
                break;

            case "Victory":
                this.SFX.PlayOneShot(this.Victory);
                break;
        }
    }
}
