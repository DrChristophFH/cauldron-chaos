using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class Speaker : MonoBehaviour {
  [SerializeField]
  private AudioClip newTextSound;
  [SerializeField]
  private TextMeshPro textMesh;
  [SerializeField]
  private float charDelay = 0.1f;
  [SerializeField]
  private ParticleSystem particles;
  [SerializeField]
  public List<NarrationPosition> narrationPositions;

  private string text;

  public void Speak(string text) {
    this.text = text;
    textMesh.text = "";
    StartCoroutine(SpeakCoroutine());
  }

  public void MoveTo(string positionName) {
    var targetPosition = narrationPositions.Find(n => n.positionName == positionName);
    
    if (targetPosition != null) {
      transform.position = targetPosition.position;
      transform.rotation = targetPosition.rotation;
    } else {
      Debug.LogError($"Position {positionName} not found!");
    }
  }

  private IEnumerator SpeakCoroutine() {
    particles.Play();
    AudioSource.PlayClipAtPoint(newTextSound, transform.position);
    foreach (char c in text) {
      textMesh.text += c;
      yield return new WaitForSeconds(charDelay);
    }
  }
}