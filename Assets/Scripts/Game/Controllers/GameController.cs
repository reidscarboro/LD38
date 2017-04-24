using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour {

    public static GameController instance;

    public enum TouchMode {
        MAIN,
        BUILD,
        BUILD_PLACE,
        BUILDING_SELECTED
    }

    //core game logic stuff
    public int power = 0;
    private int sunPowerPerClick = 1;
    private TouchMode touchMode = TouchMode.MAIN;

    //sun stuff
    public List<int> sunUpgradePrices;
    public List<string> sunUpgradeDescriptions;
    public int sunUpgradeTier = 0;

    protected List<BuildingSlot> buildingSlots;
    public List<Building> buildingPrefabs;
    public List<Building> buildings;
    public List<int> buildingPrices;

    public int buildingPlaceIndex = 0;
    public BuildingSlot selectedBuildingSlot;

    void Awake() {
        instance = this;
    }

    void Start() {
        UIController.SetPower(power);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
            Vector2 mouseDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //handle the touch event based on our game state
            if (touchMode == TouchMode.MAIN) {

                //we just touched the planet
                if (mouseDown.magnitude < 2) {
                    StartBuild();

                //we are not touching the planet, so shoot
                } else {
                    foreach (Building building in buildings) {
                        building.RequestFire(mouseDown);
                    }
                }

            } else if (touchMode == TouchMode.BUILD) {
                List<BuildingSlot> touchedBuildingSlots = new List<BuildingSlot>();
                foreach (BuildingSlot buildingSlot in buildingSlots) {
                    if (!buildingSlot.IsAvailable() && Vector2.Distance(mouseDown, buildingSlot.transform.position) < 1) {
                        touchedBuildingSlots.Add(buildingSlot);
                    }
                }
                if (touchedBuildingSlots != null && touchedBuildingSlots.Count > 0) {
                    BuildingSlot closestBuildingSlot = touchedBuildingSlots[0];
                    float closestDistance = 1000;
                    foreach (BuildingSlot buildingSlot in touchedBuildingSlots) {
                        float currentDistance = Vector2.Distance(mouseDown, buildingSlot.transform.position);
                        if (currentDistance < closestDistance) {
                            closestBuildingSlot = buildingSlot;
                            closestDistance = currentDistance;
                        }
                    }
                    Select(closestBuildingSlot);
                } else {
                    EndBuild();
                }
            } else if (touchMode == TouchMode.BUILD_PLACE) {
                List<BuildingSlot> touchedBuildingSlots = new List<BuildingSlot>();
                foreach (BuildingSlot buildingSlot in buildingSlots) {
                    if (buildingSlot.IsAvailable() && Vector2.Distance(mouseDown, buildingSlot.transform.position) < 1) {
                        touchedBuildingSlots.Add(buildingSlot);
                    }
                }
                if (touchedBuildingSlots != null && touchedBuildingSlots.Count > 0) {
                    BuildingSlot closestBuildingSlot = touchedBuildingSlots[0];
                    float closestDistance = 1000;
                    foreach (BuildingSlot buildingSlot in touchedBuildingSlots) {
                        float currentDistance = Vector2.Distance(mouseDown, buildingSlot.transform.position);
                        if (currentDistance < closestDistance) {
                            closestBuildingSlot = buildingSlot;
                            closestDistance = currentDistance;
                        }
                    }
                    Build(buildingPlaceIndex, closestBuildingSlot);
                } else {
                    StartBuild();
                }
            } else if (touchMode == TouchMode.BUILDING_SELECTED) {
                List<BuildingSlot> touchedBuildingSlots = new List<BuildingSlot>();
                foreach (BuildingSlot buildingSlot in buildingSlots) {
                    if (!buildingSlot.IsAvailable() && Vector2.Distance(mouseDown, buildingSlot.transform.position) < 1) {
                        touchedBuildingSlots.Add(buildingSlot);
                    }
                }
                if (touchedBuildingSlots != null && touchedBuildingSlots.Count > 0) {
                    BuildingSlot closestBuildingSlot = touchedBuildingSlots[0];
                    float closestDistance = 1000;
                    foreach (BuildingSlot buildingSlot in touchedBuildingSlots) {
                        float currentDistance = Vector2.Distance(mouseDown, buildingSlot.transform.position);
                        if (currentDistance < closestDistance) {
                            closestBuildingSlot = buildingSlot;
                            closestDistance = currentDistance;
                        }
                    }
                    Select(closestBuildingSlot);
                } else {
                    StartBuild();
                }
            }
        }
        
    }

    public void PressButton() {
        instance.power += instance.sunPowerPerClick;
        UIController.SetPower(power);
        
    }

    public void PlayPressButton() {
        SoundController.PlayButtonPress();
    }

    public void PlayReleaseButton() {
        SoundController.PlayButtonRelease();
    }

    public static int Power() {
        return instance.power;
    }

    public static void IncrementPower(int _power) {
        instance.power += _power;
        UIController.SetPower(instance.power);
    }

    public static void DecrementPower(int _power) {
        instance.power -= _power;
        UIController.SetPower(instance.power);
    }

    public static void RegisterBuildingSlot(BuildingSlot buildingSlot) {
        if (instance.buildingSlots == null)
            instance.buildingSlots = new List<BuildingSlot>();
        instance.buildingSlots.Add(buildingSlot);
    }

    public void StartBuild() {
        touchMode = TouchMode.BUILD;
        UIController.SetPanel(UIController.Panel.BUILD);
        CameraController.ZoomIn();

        foreach (BuildingSlot buildingSlot in buildingSlots) {
            buildingSlot.HideIndicator();
        }
    }

    public void StartBuildPlace(int buildingIndex) {
        touchMode = TouchMode.BUILD_PLACE;
        buildingPlaceIndex = buildingIndex;

        UIController.SetPanel(UIController.Panel.BUILD_PLACE);
        foreach (BuildingSlot buildingSlot in buildingSlots) {
            if (buildingSlot.IsAvailable()) {
                buildingSlot.ShowIndicator();
            } else {
                buildingSlot.HideIndicator();
            }
        }
    }

    public void Select(BuildingSlot buildingSlot) {
        touchMode = TouchMode.BUILDING_SELECTED;
        selectedBuildingSlot = buildingSlot;
        foreach (BuildingSlot buildingSlotToHide in buildingSlots) {
            buildingSlotToHide.HideIndicator();
        }
        selectedBuildingSlot.ShowIndicator();

        UIController.SetUpgradeButton(selectedBuildingSlot.child);
        UIController.SetPanel(UIController.Panel.BUILDING_SELECTED);
    }

    public void Upgrade() {
        DecrementPower(selectedBuildingSlot.child.GetUpgradePrice());
        selectedBuildingSlot.child.Upgrade();
        StartBuild();
    }

    public void UpgradeSun() {
        DecrementPower(sunUpgradePrices[sunUpgradeTier]);
        sunUpgradeTier++;

        switch (sunUpgradeTier) {
            case 0:
                sunPowerPerClick = 1;
                break;
            case 1:
                sunPowerPerClick = 2;
                break;
            case 2:
                sunPowerPerClick = 10;
                break;
            case 3:
                sunPowerPerClick = 100;
                break;
        }

        UIController.SetSunUpgradeButton();
        StartBuild();
    }

    public void Remove() {
        Destroy(selectedBuildingSlot.child.gameObject);
        selectedBuildingSlot.child = null;
        StartBuild();
    }

    public void Build(int buildingIndex, BuildingSlot buildingSlot) {
        Building building = Instantiate(buildingPrefabs[buildingIndex]);

        building.transform.SetParent(buildingSlot.transform);
        building.transform.localPosition = Vector2.zero;
        building.transform.localRotation = Quaternion.identity;
        building.transform.SetParent(buildingSlot.transform);

        buildings.Add(building);
        buildingSlot.child = building;

        DecrementPower(buildingPrices[buildingIndex]);
        SoundController.PlayClick();

        StartBuild();
    }

    public void EndBuild() {
        touchMode = TouchMode.MAIN;
        UIController.SetPanel(UIController.Panel.MAIN);
        CameraController.ZoomOut();
        foreach (BuildingSlot buildingSlot in buildingSlots) {
            buildingSlot.HideIndicator();
        }
    }

    public static void GameOver() {
        instance.StartBuild();
        UIController.GameOver();
    }
}
