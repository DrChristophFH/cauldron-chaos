using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(fileName = "CauldronState", menuName = "Cauldron Chaos/State", order = 1)]
public class CauldronState : ScriptableObject {
  [SerializeField]
  private List<CauldronTransition>  transitions = new();
  [SerializeField]
  private Material liquid;
  [SerializeField]
  private CauldronSmokeConfig smokeConfig;

  public Material Liquid => liquid;
  public CauldronSmokeConfig SmokeConfig => smokeConfig;

  public void CheckTransitions(Cauldron cauldron) {
    foreach (CauldronTransition transition in transitions) {
      if (transition.Check(cauldron)) {
        return;
      }
    }
  }
}
