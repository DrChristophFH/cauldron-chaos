using System;
using System.Collections.Generic;

using UnityEngine;

public class Cauldron : MonoBehaviour {
  private readonly Dictionary<IngredientMaterial, int> materials = new();

  [SerializeField]
  private CauldronState state;

  public Dictionary<IngredientMaterial, int> Materials => materials;

  public void AddPart(Part part) {
    foreach (IngredientMaterial material in part.Material.Affected()) {
      Materials.TryGetValue(material, out int current);
      Materials[material] = current + part.Amount;
    }

    state.CheckTransitions(this);
  }

  public void UpdateMaterials(Dictionary<IngredientMaterial, int> newMaterials) {
    foreach (KeyValuePair<IngredientMaterial, int> pair in newMaterials) {
      Materials[pair.Key] = pair.Value;
    }
  }

  public void TransitionTo(CauldronState destination) {
    Debug.Log($"Transitioning from {state.name} to {destination.name}");
    state = destination;
  }
}