using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    /*
        Enemy Types:

        Basic
            Weak, flys at planet and shoots it when close enough
        Medium 
            Basically the same thing
        Laser
            Will try to get in range and then will shoot lasers, force player to prioritize these
        Carrier
            Will post up a certain distance and spawn basic enemies
        Unnamed
            Will shoot powerful bullets that need to be destroyed with
    */

    public static EnemyController instance;

    public List<Enemy> enemies;

    public List<Enemy> enemyPrefabs;
    public List<Enemy> bossPrefabs;

    public int wave = 0;
    public float waveCooldown = 30;
    public float waveCooldownCounter = 0;

    private float difficultyScalar;

    void Awake() {
        instance = this;
    }

    void Start() {
        //InvokeRepeating("SpawnWave", waveCooldown, waveCooldown);
        waveCooldownCounter = waveCooldown;

        difficultyScalar = PlayerPrefs.GetFloat("diff");
    }

    void Update() {
        waveCooldownCounter -= Time.deltaTime;

        if (waveCooldownCounter <= 0) {
            waveCooldownCounter = waveCooldown;
            SpawnWave();
        }

        UIController.UpdateWave(wave, (int)waveCooldownCounter);
    }

    public void SpawnWave() {

        wave++;

        UIController.PulseScreen();
        SoundController.PlayWave();

        List<Vector2> spawnLocations = new List<Vector2>();
        spawnLocations.Add(new Vector2(30, 20));
        spawnLocations.Add(new Vector2(30, -20));
        spawnLocations.Add(new Vector2(-30, 20));
        spawnLocations.Add(new Vector2(-30, -20));

        //spawn enemies
        Vector2 spawnLocation = spawnLocations[Random.Range(0, 4)];

        float difficulty = Mathf.Pow(difficultyScalar, wave);
        float numEnemies = 2 + wave/2 + Random.Range(-2, 2);
        if (numEnemies <= 2) numEnemies = 2;

        for (int i = 0; i < numEnemies; i++) {
            Enemy enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Count)], spawnLocation + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)), Quaternion.identity);
            enemy.Initiate(difficulty);
        }

        if ((wave) % 10 == 0) {
            Enemy boss = Instantiate(bossPrefabs[Random.Range(0, bossPrefabs.Count)], spawnLocation + new Vector2(Random.Range(-3, 3), Random.Range(-3, 3)), Quaternion.identity);
            boss.Initiate(difficulty);
        }

       
    }

    public static List<Enemy> GetEnemies() {
        if (instance.enemies == null) instance.enemies = new List<Enemy>();
        return instance.enemies;
    }

    public static void RegisterEnemy(Enemy enemy) {
        if (instance.enemies == null) instance.enemies = new List<Enemy>();
        instance.enemies.Add(enemy);
    }

    public static void Remove(Enemy enemy) {
        instance.enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public static int GetWave() {
        return instance.wave;   
    }
}
