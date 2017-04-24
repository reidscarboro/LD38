using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeController : MonoBehaviour {

	public void StartGame(int difficulty) {
        if (difficulty == 2) {
            PlayerPrefs.SetFloat("diff", 1.3f);
        } else {
            PlayerPrefs.SetFloat("diff", 1.15f);
        }
        SceneManager.LoadScene(1);
    }
}
