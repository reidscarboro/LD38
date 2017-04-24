using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public enum ProjectileType {
        BULLET,
        LASER,
        MISSLE
    }

    public Bullet bulletPrefab;
    public Laser laserPrefab;
    public Missle misslePrefab;
    public Explosion explosionPrefab;
    public Rigidbody2D rb;
    public LayerMask friendlyLayers;

    //attributes
    public float damageBase = 1;
    public float healthBase = 100;
    public float shootCooldown = 1;
    public float bulletSpeed = 5;

    public ProjectileType projectileType;
    public float distanceToShoot = 10;

    private float damage;
    private float maxHealth;
    private float health;

    private float shootCooldownCounter = 0;

    public GameObject healthBar;
    public float healthBarOffset = 1;

    private Vector2 randomDirection = new Vector2();

    void Start() {
        health = maxHealth;
    }

    public void Initiate(float difficultyMultiplier) {
        maxHealth = healthBase * difficultyMultiplier;
        health = maxHealth;
        damage = damageBase;

        EnemyController.RegisterEnemy(this);
    }

    void Update() {
        healthBar.transform.position = transform.position - new Vector3(0, healthBarOffset, 0);
        healthBar.transform.rotation = Quaternion.identity;

        if (shootCooldownCounter > 0) {
            shootCooldownCounter -= Time.deltaTime;
            if (shootCooldownCounter < 0) shootCooldownCounter = 0;
        } else {
            if (transform.position.magnitude < distanceToShoot) {
                //if we raycast our target
                if (Physics2D.Raycast(transform.position, -transform.position, distanceToShoot, friendlyLayers)) {
                    shootCooldownCounter = shootCooldown;
                    Shoot();
                }
            }
        }
    }

    void FixedUpdate() {
        //rules
        float moveTowardsForce = 5;
        float moveAwayDistance = 10;
        float moveAwayMaxForce = 30;
        float maxRandomForce = 50;


        //move towards planet
        rb.AddForce(-transform.position * moveTowardsForce);

        //but not too close
        if (transform.position.magnitude < moveAwayDistance) {
            rb.AddForce(transform.position.normalized * (moveAwayMaxForce * (moveAwayDistance - transform.position.magnitude)));
        }

        //move randomly otherwise
        if (Random.Range(0.0f, 1.0f) > 0.99f) {
            randomDirection = new Vector2(Random.Range(-maxRandomForce, maxRandomForce), Random.Range(-maxRandomForce, maxRandomForce));
        }
        rb.AddForce(randomDirection);

        AimAt(Vector2.zero);
    }

    private void Shoot() {
        switch (projectileType) {
            case ProjectileType.BULLET:
                Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                bullet.Initiate(-transform.position, damage, bulletSpeed);
                SoundController.PlayShoot();
                break;

            case ProjectileType.LASER:
                RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.position, distanceToShoot, friendlyLayers);
                Vector2 end = Vector2.zero;
                if (hit) {
                    Planet planet = hit.collider.gameObject.GetComponent<Planet>();
                    if (planet != null) {
                        planet.Damage(damage);
                    }
                    end = hit.point;
                    Laser laser = Instantiate(laserPrefab, transform.position, Quaternion.identity);
                    laser.Initiate(new Vector2(transform.position.x, transform.position.y), end);
                }
                SoundController.PlayLaser();
                break;

            case ProjectileType.MISSLE:
                Missle missle = Instantiate(misslePrefab, transform.position, Quaternion.identity);
                missle.Initiate(-transform.position, new Vector2(transform.position.x, transform.position.y), Vector2.zero, damage, bulletSpeed, 2);
                SoundController.PlayShoot();
                break;
        }
    }

    public void Damage(float damage) {
        health -= damage;
        healthBar.transform.localScale = new Vector3(health / maxHealth, 0.1f, 1);

        if (health <= 0) {
            EnemyController.Remove(this);
            Explosion explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.Initiate(transform.localScale.x * 2);
        }
    }

    public void AimAt(Vector2 position) {
        Vector2 direction = (position - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
    }
}
