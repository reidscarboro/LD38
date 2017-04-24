using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missle : MonoBehaviour {

    public Rigidbody2D rb;
    public LineRenderer lineRenderer;
    public Explosion explosionPrefab;

    private float lifeSpan = 10;
    private float timeAlive = 0;

    private Vector2 velocityVector;

    private Vector2 start;
    private Vector2 end;
    private float explosionSize;

    private float damage;

    public LayerMask collisionLayer;

    private float lastDistance = 1000;

    public void Initiate(Vector2 direction, Vector2 _start, Vector2 _end, float _damage, float speed, float _explosionSize) {
        start = _start;
        end = _end;
        explosionSize = _explosionSize;
        damage = _damage;

        velocityVector = direction.normalized * speed;
        rb.velocity = velocityVector;
        rb.MoveRotation(-Math.Angle(direction));

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.enabled = true;
    }

    void Update() {
        if (timeAlive > lifeSpan) {
            Destroy(gameObject);
        } else {
            timeAlive += Time.deltaTime;

            float distance = Vector2.Distance(transform.position, end);
            if (distance > lastDistance) {
                Explode();
            } else {
                lastDistance = distance;
            }

            lineRenderer.SetPosition(0, start);
            lineRenderer.SetPosition(1, transform.position);
        }
    }

    private void Explode() {
        SoundController.PlayHit();
        //explosion visual
        Vector3 position = transform.position;
        position.z = -5;
        Explosion explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        explosion.Initiate(explosionSize);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, explosionSize, collisionLayer);
        if (colliders.Length > 0) {
            foreach (Collider2D collider in colliders) {
                //check for enemies, do damage
                Enemy enemy = collider.gameObject.GetComponent<Enemy>();
                if (enemy != null) enemy.Damage(damage);

                //check for enemy bullets
                Bullet bullet = collider.gameObject.GetComponent<Bullet>();
                if (bullet != null) Destroy(bullet.gameObject);

            }
        }

        Destroy(gameObject);
    }

    //if we collide with something on our collision layer, do damage to that object
    void OnCollisionEnter2D(Collision2D coll) {
        if (collisionLayer == (collisionLayer | (1 << coll.gameObject.layer))) {
            Explode();
        }
    }
}
