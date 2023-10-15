using Unity.VisualScripting;

using UnityEngine;

public class InspectionStation : MonoBehaviour {
  [SerializeField]
  private Speaker speaker;
  [SerializeField]
  private ParticleSystem inspectParticles;

  [SerializeField]
  private string inspectionText = "Inspecting...";

  [SerializeField]
  private AudioClip inspectSound;
  [SerializeField]
  private AudioClip inspectSuccessSound;

  private bool isInspecting = false;

  private void Start() {
    inspectParticles.Stop();
  }

  private async void OnTriggerEnter(Collider other) {
    if (isInspecting) {
      return;
    }
    if (other.gameObject.TryGetComponent(out Ingredient ingredient)) {
      isInspecting = true;
      inspectParticles.Play();
      AudioSource.PlayClipAtPoint(inspectSound, transform.position);
      await speaker.Speak(inspectionText, 0.4f);
      inspectParticles.Stop();
      AudioSource.PlayClipAtPoint(inspectSuccessSound, transform.position);
      await speaker.Speak(ingredient.GetDescription());
      isInspecting = false;
    }
  }

}