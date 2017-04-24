using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Rigidbody2D rb;

    public LayerMask collisionLayer;

    private float lifeSpan = 10;
    private float timeAlive = 0;

    private float damage;

    private Vector2 velocityVector;

    public void Initiate(Vector2 direction, float _damage, float speed) {
        damage = _damage;
        velocityVector = direction.normalized * speed;
        rb.velocity = velocityVector;
        rb.MoveRotation(-Math.Angle(direction));
    }

    void Update() {
        if (timeAlive > lifeSpan) {
            Destroy(gameObject);
        } else {
            timeAlive += Time.deltaTime;
        }
    }

    //if we collide with something on our collision layer, do damage to that object
    void OnCollisionEnter2D(Collision2D coll) {
        if (collisionLayer == (collisionLayer | (1 << coll.gameObject.layer))) {
            Enemy enemy = coll.gameObject.GetComponent<Enemy>();
            if (enemy != null) {
                enemy.Damage(damage);
                SoundController.PlayHit();
            }

            Planet planet = coll.gameObject.GetComponent<Planet>();
            if (planet != null) {
                planet.Damage(damage);
                SoundController.PlayHit();
            }
            Destroy(gameObject);
        }
    }
}
