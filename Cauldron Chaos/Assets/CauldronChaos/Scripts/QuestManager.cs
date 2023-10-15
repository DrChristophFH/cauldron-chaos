using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour, IObserver<CauldronState> {

  [SerializeField]
  private List<Quest> quests;

  [SerializeField]
  private Cauldron cauldron;
  private IObservable<CauldronState> cauldronState;

  [SerializeField]
  private CauldronState gameOverState;
  [SerializeField]
  private GameObject gameOverPanel;

  [SerializeField]
  private Quest currentQuest;

  [SerializeField]
  private AudioClip successSound;
  [SerializeField]
  private AudioClip newQuestSound;
  [SerializeField] 
  private AudioClip gameOverSound;

  [SerializeField]
  private Speaker speaker;

  public int Score { get; private set; } = 0;

  private void Start() {
    cauldronState = cauldron;
    cauldronState.Subscribe(this);
    Vector3 panelPosition = gameOverPanel.transform.position;
    panelPosition.y -= 5;
    gameOverPanel.transform.position = panelPosition;
    gameOverPanel.SetActive(false);
    Invoke("GenerateNewRandomQuest", 5f);
  }

  public void OnCompleted() { } // do nothing
  public void OnError(Exception error) { } // do nothing

  public void OnNext(CauldronState newState) {
    if (currentQuest.RequiredCauldronState.Equals(newState)) {
      Score += currentQuest.RewardPoints;
      AudioHelper.PlayClipAtPointWithSettings(successSound, transform.position);
      Invoke("GenerateNewRandomQuest", 5f);
    } else if (gameOverState.Equals(newState)) {
      gameOverPanel.SetActive(true);
      AudioHelper.PlayClipAtPointWithSettings(gameOverSound, transform.position);
      speaker.Speak($"Game Over: {Score} points", 0.2f);
      StartCoroutine(FlyInGameOverPanel());
    }
  }

  private IEnumerator FlyInGameOverPanel() {
    var transform = gameOverPanel.GetComponent<Transform>();
    Vector3 originalPosition = gameOverPanel.GetComponent<Transform>().position;
    var targetPosition = new Vector3(originalPosition.x, originalPosition.y + 5, originalPosition.z);
    var t = 0f;
    while (t < 1f) {
      t += Time.deltaTime;
      transform.position = Vector3.Lerp(originalPosition, targetPosition, t);
      yield return null;
    }
  }

  private void GenerateNewRandomQuest() {
    int randomIndex = UnityEngine.Random.Range(0, quests.Count);
    cauldron.Reset();
    currentQuest = quests[randomIndex];
    AudioHelper.PlayClipAtPointWithSettings(newQuestSound, transform.position);
    speaker.Speak(currentQuest.QuestName);
  }
}
