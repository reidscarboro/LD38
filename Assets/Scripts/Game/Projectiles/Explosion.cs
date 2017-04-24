using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Explosion : MonoBehaviour {

    public float scaleTime = 0.5f;

    void Update() {
        if (transform.localScale.x <= 0.1f) {
            Destroy(gameObject);
        }
    }

    public void Initiate(float size) {
        transform.localScale = new Vector3(size, size, size);
        transform.DOScale(0, scaleTime * transform.localScale.x);
    }
}
