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
  private IObservable<CauldronState> cauldron;

  [SerializeField]
  private Quest currentQuest;

  [SerializeField]
  private Speaker speaker;

  public int Score { get; private set; } = 0;

  private void Start() {
    cauldron.Subscribe(this);
    GenerateNewRandomQuest();
  }

  public void OnCompleted() { } // do nothing
  public void OnError(Exception error) { } // do nothing
  
  public void OnNext(CauldronState newState) {
    if (currentQuest.RequiredCauldronState.Equals(newState)) {
      Score += currentQuest.RewardPoints;
      GenerateNewRandomQuest();
      speaker.Speak(currentQuest.Description);
    }
  }

  private void GenerateNewRandomQuest() {
    int randomIndex = UnityEngine.Random.Range(0, quests.Count);
    currentQuest = quests[randomIndex];
  }
}
