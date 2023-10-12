using UnityEngine;

public class Speaker : MonoBehaviour {
  [SerializeField]
  private AudioClip newTextSound;
  [SerializeField]
  private TextMesh textMesh;
  [SerializeField]
  private float charDelay = 0.1f;
  [SerializeField]
  private ParticleSystem particleSystem;
  [SerializeField]
  public List<NarrationPosition> narrationPositions;

  private string text;

  public void Speak(string text) {
    this.text = text;
    textMesh.text = "";
    StartCoroutine(SpeakCoroutine());
  }

  public void MoveTo(string positionName) {
    var targetPosition = narrationPositions.FirstOrDefault(n => n.positionName == positionName);
    
    if (targetPosition != null) {
      transform.position = targetPosition.position;
      transform.rotation = targetPosition.rotation;
      OnMoveComplete?.Invoke();
    } else {
      Debug.LogError($"Position {positionName} not found!");
    }
  }

  private System.Collections.IEnumerator SpeakCoroutine() {
    foreach (char c in text) {
      textMesh.text += c;
      yield return new WaitForSeconds(textSpeed);
    }
    particleSystem.Play();
    AudioSource.PlayClipAtPoint(newTextSound, transform.position);
  }
}