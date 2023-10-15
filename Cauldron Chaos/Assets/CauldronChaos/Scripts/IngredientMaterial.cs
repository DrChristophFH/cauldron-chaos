using System;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

[CreateAssetMenu(fileName = "IngredientMaterial", menuName = "Cauldron Chaos/Ingredient Material", order = 0)]
public class IngredientMaterial : ScriptableObject {
  public IngredientMaterial ParentMaterial;
  
  public List<IngredientMaterial> Children = new();

  public List<IngredientMaterial> Affected() {
    List<IngredientMaterial> affected = new() {
      this
    };

    var parent = ParentMaterial;
    while (parent != null) {
      affected.Add(parent);
      parent = parent.ParentMaterial;
    }

    return affected;
  }

  public void AddChild(IngredientMaterial child) {
    if (!Children.Contains(child)) {
      Children.Add(child);
      child.ParentMaterial = this;
    }
  }

  public void ClearChildren() {
    Children.Clear();
  }

  public static IngredientMaterial GetMaterial(string name) {
    return Resources.Load<IngredientMaterial>($"CauldronChaos/Data/Materials/{name}");
  }

  public override string ToString() {
    return name;
  }
}

