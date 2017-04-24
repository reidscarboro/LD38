using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

    public static SoundController instance;

    public AudioClip buttonPress;
    public AudioClip buttonRelease;
    public AudioClip shoot;
    public AudioClip laser;
    public AudioClip click;
    public AudioClip waveSpawn;

    public AudioSource audioSource;

    void Awake() {
        instance = this;
    }

    public static void PlayButtonPress() {
        instance.audioSource.clip = instance.buttonPress;
        instance.audioSource.volume = 0.5f;
        instance.audioSource.pitch = Random.Range(0.9f, 1.1f);
        instance.audioSource.Play();
    }
    public static void PlayButtonRelease() {
        instance.audioSource.clip = instance.buttonRelease;
        instance.audioSource.volume = 0.5f;
        instance.audioSource.pitch = Random.Range(0.9f, 1.1f);
        instance.audioSource.Play();
    }
    public static void PlayShoot() {
        instance.audioSource.clip = instance.shoot;
        instance.audioSource.volume = 1;
        instance.audioSource.pitch = Random.Range(0.9f, 1.1f);
        instance.audioSource.Play();
    }
    public static void PlayLaser() {
        instance.audioSource.clip = instance.laser;
        instance.audioSource.volume = 1;
        instance.audioSource.pitch = Random.Range(0.9f, 1.1f);
        instance.audioSource.Play();
    }
    public static void PlayHit() {
        instance.audioSource.clip = instance.shoot;
        instance.audioSource.volume = 1;
        instance.audioSource.pitch = Random.Range(0.7f, 0.9f);
        instance.audioSource.Play();
    }
    public static void PlayClick() {
        instance.audioSource.clip = instance.click;
        instance.audioSource.volume = 1;
        instance.audioSource.pitch = Random.Range(0.7f, 0.9f);
        instance.audioSource.Play();
    }
    public static void PlayWave() {
        instance.audioSource.clip = instance.waveSpawn;
        instance.audioSource.volume = 1;
        instance.audioSource.pitch = Random.Range(0.7f, 0.9f);
        instance.audioSource.Play();
    }
}
