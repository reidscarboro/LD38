using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Laser : MonoBehaviour {

    public LineRenderer lineRenderer;

    private float lifeSpan = 0.5f;
    private float timeAlive = 0;

    void Update() {
        if (timeAlive > lifeSpan) {
            Destroy(gameObject);
        } else {
            timeAlive += Time.deltaTime;
        }
    }

    public void Initiate(Vector2 start, Vector2 end) {
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
        
        Color2 fromColor = new Color2();
        fromColor.ca = lineRenderer.startColor;
        fromColor.cb = lineRenderer.startColor;

        Color2 toColor = new Color2();
        toColor.ca = lineRenderer.startColor;
        toColor.cb = lineRenderer.startColor;
        toColor.ca.a = 0;
        toColor.cb.a = 0;

        lineRenderer.DOColor(fromColor, toColor, lifeSpan);
        lineRenderer.enabled = true;
    }
}
