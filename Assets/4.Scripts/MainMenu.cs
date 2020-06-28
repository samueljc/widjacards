using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
  public int playSceneIndex;

  public void Play() {
    SceneManager.LoadScene(playSceneIndex);
  }

  public void Quit() {
    Debug.Log("MainMenu::Quit");
    Application.Quit();
  }
}
