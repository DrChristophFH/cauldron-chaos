using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

public class ResetButton : MonoBehaviour {


  public void OnButtonDown(Hand fromHand) {
    fromHand.TriggerHapticPulse(1000);
    // reset scene
    Scene scene = SceneManager.GetActiveScene();
    SceneManager.LoadScene(scene.name);
  }
}