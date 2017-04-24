using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerGenerator : Building {

    private float tickTime = 1;
    private float tickCounter = 0;

    public List<int> power;

    void Update() {
        tickCounter -= Time.deltaTime;

        if (tickCounter <= 0) {
            GameController.IncrementPower(power[upgradeTier]);
            tickCounter = tickTime;
        }
    }

    public override void RequestFire(Vector2 crosshair) {
        //dont do anything
    }
}
