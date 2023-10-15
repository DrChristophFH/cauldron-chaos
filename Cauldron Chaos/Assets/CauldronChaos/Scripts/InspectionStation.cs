using CartoonFX;

using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InspectionStation : MonoBehaviour {
  [SerializeField]
  private Speaker speaker;
  [SerializeField]
  private GameObject inspectParticles;
  [SerializeField] 
  private GameObject hintMarker;
  [SerializeField]
  private float hintMarkerRotationSpeed = 0.1f;

  [SerializeField]
  private string inspectionText = "Inspecting...";

  [SerializeField]
  private AudioClip inspectSound;
  [SerializeField]
  private AudioClip inspectSuccessSound;

  private bool isInspecting = false;

  private void Start() {
    inspectParticles.SetActive(false);
  }

  private void Update() {
    if (hintMarker.activeSelf) {
      hintMarker.transform.Rotate(Vector3.up, hintMarkerRotationSpeed * Time.deltaTime);
    }
  }

  private async void OnTriggerEnter(Collider other) {
    if (isInspecting) {
      return;
    }
    if (other.gameObject.TryGetComponent(out Ingredient ingredient)) {
      hintMarker.SetActive(false);
      isInspecting = true;
      AudioHelper.PlayClipAtPointWithSettings(inspectSound, transform.position);
      inspectParticles.SetActive(true);
      await speaker.Speak(inspectionText, 0.4f);
      inspectParticles.SetActive(false);
      AudioHelper.PlayClipAtPointWithSettings(inspectSuccessSound, transform.position);
      await speaker.Speak(ingredient.GetDescription());
      isInspecting = false;
    }
  }

}