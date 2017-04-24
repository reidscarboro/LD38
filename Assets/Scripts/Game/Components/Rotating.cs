using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour {

    public float speed = 1f;

	void Update () {
        float dt = Time.deltaTime;
        transform.Rotate(Vector3.forward, speed * dt);
	}
}
