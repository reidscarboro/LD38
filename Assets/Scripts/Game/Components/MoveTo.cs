using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour {

    public Transform parent;

    void Update() {
        transform.position = parent.position;
        transform.rotation = Quaternion.Euler(0, 0, 135);
    }
}
