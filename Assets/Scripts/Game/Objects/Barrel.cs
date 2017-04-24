using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour {

    public bool autoAim = false;

	// Update is called once per frame
	void Update () {
        if (!autoAim) {
            Vector2 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // get direction you want to point at
            Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

            // set vector of transform directly
            transform.up = direction;
        }
    }

    public void AimAt(Vector2 position) {
        Vector2 direction = (position - (Vector2)transform.position).normalized;

        // set vector of transform directly
        transform.up = direction;
    }
}
