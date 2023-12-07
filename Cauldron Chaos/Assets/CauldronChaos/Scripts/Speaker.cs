using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using TMPro;

using UnityEngine;

public class Speaker : MonoBehaviour {
  [SerializeField]
  private TextMeshPro textMesh;
  [SerializeField]
  private float defaultCharDelay = 0.1f;
  [SerializeField]
  private ParticleSystem particles;
  [SerializeField]
  public List<NarrationPosition> narrationPositions;

  private string text;

  private TaskCompletionSource<bool> tcs;

  private void Start() {
    if (particles != null) {
      particles.Stop();
    }
    textMesh.text = "";
  }

  public Task Speak(string text) {
    return Speak(text, defaultCharDelay);
  }

  public Task Speak(string text, float charDelay) {
    this.text = text;
    textMesh.text = "";
    tcs = new TaskCompletionSource<bool>();
    StartCoroutine(SpeakCoroutine(charDelay));
    return tcs.Task;
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

  private IEnumerator SpeakCoroutine(float charDelay) {
    if (particles != null) {
      particles.Play();
    }
    foreach (char c in text) {
      textMesh.text += c;
      yield return new WaitForSeconds(charDelay);
    }
    tcs.SetResult(true);
  }
}