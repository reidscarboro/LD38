using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour {

    public enum Panel {
        MAIN,
        BUILD,
        BUILD_PLACE,
        BUILDING_SELECTED
    }

    public static UIController instance;

    public GameObject panelMain;
    public GameObject panelBuild;
    public GameObject panelBuildPlace;
    public GameObject panelBuildingSelected;
    public GameObject panelGameOver;

    public Text power;
    public Text helpMessage;

    public Button upgradeButton;
    public Text upgradeButtonTitle;
    public Text upgradeButtonDescription;
    public Text upgradeButtonPrice;

    public Button sunUpgradeButton;
    public Text sunUpgradeButtonTitle;
    public Text sunUpgradeButtonDescription;
    public Text sunUpgradeButtonPrice;

    public List<Button> buildingButtons;

    public Building selectedBuilding;

    public Text waveCounter;
    public Text waveCountdown;

    public Image pulse;
    public Color pulseFrom;
    public Color pulseTo;

    public Text gameOverText;

    void Awake() {
        instance = this;
    }

    public static void SetPanel(Panel panel) {
        instance.panelMain.SetActive(panel == Panel.MAIN);
        instance.panelBuild.SetActive(panel == Panel.BUILD);
        instance.panelBuildPlace.SetActive(panel == Panel.BUILD_PLACE);
        instance.panelBuildingSelected.SetActive(panel == Panel.BUILDING_SELECTED);

        switch (panel) {
            case Panel.MAIN:
                instance.helpMessage.text = "";
                break;
            case Panel.BUILD:
                instance.helpMessage.text = "Build a new unit or upgrade an existing one";
                break;
            case Panel.BUILD_PLACE:
                instance.helpMessage.text = "";
                break;
            case Panel.BUILDING_SELECTED:
                instance.helpMessage.text = instance.selectedBuilding.name;
                break;
        }
    }

    void FixedUpdate() {
        pulse.color = Color.Lerp(pulse.color, pulseTo, 0.025f);
    }

    public static void SetPower(int power) {
        instance.power.text = power.ToString();
        UpdateUpgradeButtons();
    }

    public static void SetUpgradeButton(Building _selectedBuilding) {
        instance.selectedBuilding = _selectedBuilding;
        if (instance.selectedBuilding.upgradeTier < 3) {
            instance.upgradeButtonTitle.text = "Level " + instance.selectedBuilding.upgradeTier.ToString() + " > " + "Level " + (instance.selectedBuilding.upgradeTier + 1).ToString();
            instance.upgradeButtonDescription.text = instance.selectedBuilding.GetUpgradeDescription();
            instance.upgradeButtonPrice.text = instance.selectedBuilding.GetUpgradePrice().ToString();
        }
        UpdateUpgradeButtons();
    }

    public static void SetSunUpgradeButton() {
        if (GameController.instance.sunUpgradeTier < 3) {
            
            instance.sunUpgradeButtonDescription.text = GameController.instance.sunUpgradeDescriptions[GameController.instance.sunUpgradeTier];
            instance.sunUpgradeButtonPrice.text = GameController.instance.sunUpgradePrices[GameController.instance.sunUpgradeTier].ToString();
        }
        UpdateUpgradeButtons();
    }

    public static void UpdateUpgradeButtons() {
        instance.upgradeButton.interactable = (
            instance.selectedBuilding != null &&
            instance.selectedBuilding.upgradeTier < 3 &&
            GameController.Power() > instance.selectedBuilding.GetUpgradePrice()
        );
        instance.sunUpgradeButton.interactable = (
            GameController.instance.sunUpgradeTier < 3 &&
            GameController.Power() > GameController.instance.sunUpgradePrices[GameController.instance.sunUpgradeTier]
        );
        for (int i = 0; i < instance.buildingButtons.Count; i++) {
            instance.buildingButtons[i].interactable = GameController.Power() > GameController.instance.buildingPrices[i];
        }
    }

    public static void UpdateWave(int wave, int secondsLeft) {
        instance.waveCounter.text = "Wave " + wave.ToString() + " - Next in ";

        if (secondsLeft > 9) {
            instance.waveCountdown.text = "0:" + secondsLeft.ToString();
        } else {
            instance.waveCountdown.text = "0:0" + secondsLeft.ToString();
        }
    }

    public static void PulseScreen() {
        instance.pulse.color = instance.pulseFrom;
    }

    public static bool IsGameOver() {
        return instance.panelGameOver.active;
    }

    public static void GameOver() {
        instance.gameOverText.text = "You made it to wave " + EnemyController.GetWave();
        instance.panelGameOver.SetActive(true);
    }

    public void NewGame() {
        SceneManager.LoadScene(1);
    }

    public void MainMenu() {
        SceneManager.LoadScene(0);
    }
}
