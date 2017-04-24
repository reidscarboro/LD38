using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    public int numSlots = 3;
    public BuildingSlot buildingSlotPrefab;

    public bool primary = false;
    public float health = 100;
    public GameObject healthBar;

    void Start() {
        InitializeSlots();
        InvokeRepeating("RegenHealth", 0, 1);
    }

    void Update() {

    }

    public void RegenHealth() {
        if (health < 100) {
            health += 1;
            healthBar.transform.localScale = new Vector3(health / 100, 1, 1);
        }
    }

    public void InitializeSlots() {
        for (int i = 0; i < numSlots; i++) {
            float r = 1;
            float theta = i * 360 / numSlots;
            Vector2 slotPosition = Math.PolarToCartesian(r, Mathf.Deg2Rad * theta);
            BuildingSlot buildingSlot = Instantiate(buildingSlotPrefab);

            buildingSlot.transform.SetParent(transform);
            buildingSlot.transform.localPosition = slotPosition;
            buildingSlot.transform.Rotate(Vector3.forward, theta - 90);

            GameController.RegisterBuildingSlot(buildingSlot);
        }
    }

    public void Damage(float damage) {
        if (primary) {
            health -= damage;
            healthBar.transform.localScale = new Vector3(health / 100, 1, 1);

            if (health < 0) {
                healthBar.SetActive(false);
                if (!UIController.IsGameOver()) GameController.GameOver();
            }
        }
    }
}
