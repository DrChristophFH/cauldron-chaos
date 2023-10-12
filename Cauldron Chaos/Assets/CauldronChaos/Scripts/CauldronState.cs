using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "CauldronState", menuName = "Cauldron Chaos/State", order = 1)]
public class CauldronState : ScriptableObject {
  [SerializeField]
  private List<CauldronTransition> transitions = new();
  [SerializeField]
  private bool noOverwrite = false;
  [SerializeField]
  private CauldronSmokeConfig smokeConfig;
  [SerializeField]
  private CauldronContentConfig contentConfig;

  public CauldronSmokeConfig SmokeConfig => smokeConfig;
  public CauldronContentConfig ContentConfig => contentConfig;
  public bool NoOverwrite { get => noOverwrite; set => noOverwrite = value; }

  public void CheckTransitions(Cauldron cauldron) {
    foreach (CauldronTransition transition in transitions) {
      if (transition.Check(cauldron)) {
        return;
      }
    }
  }

  public void AddTransition(CauldronTransition transition) {
    transitions.Add(transition);
  }

  public void ClearTransitions() {
    transitions.Clear();
  }
}
