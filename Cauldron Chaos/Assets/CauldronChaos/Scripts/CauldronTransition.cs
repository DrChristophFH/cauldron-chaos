using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[Serializable]
public class CauldronTransition {
  [SerializeField]
  private CauldronState destination;
  [SerializeField]
  private List<Part> requiredMaterials = new();

  public bool Check(Cauldron cauldron) {
    Dictionary<IngredientMaterial, int> materials = new(cauldron.Materials);
    
    foreach (Part part in requiredMaterials) {
      foreach (IngredientMaterial material in part.Material.Affected()) {
        if (!materials.TryGetValue(material, out int current)) {
          return false;
        }

        if (current < part.Amount) {
          return false;
        }

        materials[material] = current - part.Amount;
      }
    }

    cauldron.UpdateMaterials(materials);
    cauldron.TransitionTo(destination);
    return true;
  }
}