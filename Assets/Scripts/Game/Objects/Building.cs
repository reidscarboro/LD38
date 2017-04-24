using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building : MonoBehaviour {

    public string name;
    public List<int> upgradePrices;
    public List<string> upgradeDescriptions;
    public int upgradeTier = 0;
    public List<GameObject> upgradeIndicators;

    public abstract void RequestFire(Vector2 crosshair);

    public void Upgrade() {
        if (upgradeTier < 3) {
            upgradeTier += 1;
            for (int i = 0; i < upgradeTier; i++) {
                upgradeIndicators[i].SetActive(true);
            }
        }
    }

    public int GetUpgradePrice() {
        return upgradePrices[upgradeTier];
    }

    public string GetUpgradeDescription() {
        return upgradeDescriptions[upgradeTier];
    }
}
