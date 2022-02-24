using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public Manager GameManager;
    public AudioClip enemyHit, ballHit, charge, powerUp;
    AudioSource enemyHitSource, ballHitSource, chargeSource, powerUpSource;

    void Start() {
        AudioSource[] sources = gameObject.GetComponents<AudioSource>();
        enemyHitSource = sources[0];
        ballHitSource = sources[1];
        chargeSource = sources[2];
        powerUpSource = sources[3];
    }

    public void PlayEnemyHit() {
        if (GameManager.soundEnabled < 0) return;
        enemyHitSource.PlayOneShot(enemyHit);
    }
    public void PlayBallHit() {
        if (GameManager.soundEnabled < 0) return;
        ballHitSource.PlayOneShot(ballHit);
    }
    public void PlayCharge() {
        if (GameManager.soundEnabled < 0) return;
        chargeSource.clip = charge;
        if (!chargeSource.isPlaying) chargeSource.Play();
    }
    public void StopCharge() {
        if (GameManager.soundEnabled < 0) return;
        chargeSource.Stop();
    }
    public void PlayPowerUp() {
        if (GameManager.soundEnabled < 0) return;
        powerUpSource.PlayOneShot(powerUp);
    }
}