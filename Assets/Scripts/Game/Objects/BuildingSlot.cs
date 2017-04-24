using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSlot : MonoBehaviour {

    public Building child;
    public GameObject availablilityIndicator;

    public void ShowIndicator() {
        availablilityIndicator.SetActive(true);
    }

    public void HideIndicator() {
        availablilityIndicator.SetActive(false);
    }

    public bool IsAvailable() {
        return child == null;
    }
}
