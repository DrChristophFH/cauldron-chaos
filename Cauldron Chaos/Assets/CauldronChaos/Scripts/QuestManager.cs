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
  private Quest currentQuest;

  [SerializeField]
  private AudioClip successSound;
  [SerializeField]
  private AudioClip newQuestSound;

  [SerializeField]
  private Speaker speaker;

  public int Score { get; private set; } = 0;

  private void Start() {
    cauldronState = cauldron;
    cauldronState.Subscribe(this);
    // wait 5 seconds
    Invoke("GenerateNewRandomQuest", 5f);
  }

  public void OnCompleted() { } // do nothing
  public void OnError(Exception error) { } // do nothing
  
  public void OnNext(CauldronState newState) {
    if (currentQuest.RequiredCauldronState.Equals(newState)) {
      Score += currentQuest.RewardPoints;
      AudioSource.PlayClipAtPoint(successSound, transform.position);
      Invoke("GenerateNewRandomQuest", 5f);
    }
  }

  private void GenerateNewRandomQuest() {
    int randomIndex = UnityEngine.Random.Range(0, quests.Count);
    currentQuest = quests[randomIndex];
    AudioSource.PlayClipAtPoint(newQuestSound, transform.position);
    speaker.Speak(currentQuest.QuestName);
  }
}
