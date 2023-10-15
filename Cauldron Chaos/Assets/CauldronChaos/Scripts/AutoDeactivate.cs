using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AutoDeactivate : MonoBehaviour {
  private AudioSource audioSource;

  private void Awake() {
    audioSource = GetComponent<AudioSource>();
  }

  private void Update() {
    if (!audioSource.isPlaying) {
      gameObject.SetActive(false);
    }
  }
}