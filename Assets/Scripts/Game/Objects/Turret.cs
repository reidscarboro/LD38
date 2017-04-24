using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : Building {

    public enum ProjectileType {
        BULLET,
        LASER,
        MISSLE
    }

    public Bullet bulletPrefab;
    public Laser laserPrefab;
    public Missle misslePrefab;

    public LayerMask friendlyLayers;
    public LayerMask enemyLayers;

    public Barrel barrel;

    public ProjectileType projectileType;
    public bool autoAim = false;

    //upgrade values
    public List<float> fireRate;
    public List<float> bulletSpeed;
    public List<float> damage;
    public List<float> explosionRadius;

    private bool shotQueued = false;
    private Vector2 queuedShotCrosshair;
    private float shootCooldownCounter = 0;

    public Transform projectileSpawn;

    public override void RequestFire(Vector2 crosshair) {
        if (!autoAim) {
            if (shootCooldownCounter < 0.25f) {
                shotQueued = true;
                queuedShotCrosshair = crosshair;
            }
        }
    }

    public void RequestAutoFire(Vector2 crosshair) {
        if (shootCooldownCounter < 0.25f) {
            shotQueued = true;
            queuedShotCrosshair = crosshair;
        }
    }

    void Update() {
        Enemy closestEnemy = GetClosestEnemy();
        if (closestEnemy != null && barrel != null) barrel.AimAt(closestEnemy.transform.position);

        if (shootCooldownCounter > 0) {
            shootCooldownCounter -= Time.deltaTime;
            if (shootCooldownCounter < 0) shootCooldownCounter = 0;
        } else {
            if (autoAim) {
                if (closestEnemy != null && Vector2.Distance(transform.position, closestEnemy.transform.position) < 18) {
                    RequestAutoFire(closestEnemy.transform.position);
                }
            }
        }

        if (shotQueued && shootCooldownCounter <= 0) {

            shotQueued = false;
            shootCooldownCounter = fireRate[upgradeTier];



            if (!Physics2D.Raycast(projectileSpawn.position, queuedShotCrosshair - new Vector2(projectileSpawn.position.x, projectileSpawn.position.y), 40, friendlyLayers)) {
                switch (projectileType) {
                    case ProjectileType.BULLET:
                        Bullet bullet = Instantiate(bulletPrefab, projectileSpawn.position, Quaternion.identity);
                        bullet.Initiate(queuedShotCrosshair - new Vector2(projectileSpawn.position.x, projectileSpawn.position.y), damage[upgradeTier], bulletSpeed[upgradeTier]);
                        SoundController.PlayShoot();
                        break;

                    case ProjectileType.LASER:
                        RaycastHit2D hit = Physics2D.Raycast(projectileSpawn.position, queuedShotCrosshair - new Vector2(projectileSpawn.position.x, projectileSpawn.position.y), 40, enemyLayers);
                        Vector2 end = Vector2.zero;
                        SoundController.PlayLaser();
                        if (hit) {
                            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
                            if (enemy != null) {
                                enemy.Damage(damage[upgradeTier]);
                            }
                            end = hit.point;
                            Laser laser = Instantiate(laserPrefab, projectileSpawn.position, Quaternion.identity);
                            laser.Initiate(new Vector2(projectileSpawn.position.x, projectileSpawn.position.y), end);
                        } else {
                            end = queuedShotCrosshair - new Vector2(projectileSpawn.position.x, projectileSpawn.position.y);
                            end.Normalize();

                            //max laser distance
                            end *= 40;
                            Laser laser = Instantiate(laserPrefab, projectileSpawn.position, Quaternion.identity);
                            laser.Initiate(new Vector2(projectileSpawn.position.x, projectileSpawn.position.y), new Vector2(projectileSpawn.position.x, projectileSpawn.position.y) + end);
                        }
                        break;

                    case ProjectileType.MISSLE:
                        Missle missle = Instantiate(misslePrefab, projectileSpawn.position, Quaternion.identity);
                        missle.Initiate(queuedShotCrosshair - new Vector2(projectileSpawn.position.x, projectileSpawn.position.y), new Vector2(projectileSpawn.position.x, projectileSpawn.position.y), queuedShotCrosshair, damage[upgradeTier], bulletSpeed[upgradeTier], explosionRadius[upgradeTier]);
                        SoundController.PlayShoot();
                        break;
                }
            }
        }
    }

    public Enemy GetClosestEnemy() {
        List<Enemy> enemies = EnemyController.GetEnemies();
        if (enemies.Count > 0) {
            Enemy closestEnemy = enemies[0];
            foreach (Enemy enemy in enemies) {
                if (Vector2.Distance(transform.position, enemy.transform.position) < Vector2.Distance(transform.position, closestEnemy.transform.position)) {
                    closestEnemy = enemy;
                }
            }
            return closestEnemy;
        } else {
            return null;
        }
        
    }
}
