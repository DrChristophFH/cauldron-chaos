using System;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

[CreateAssetMenu(fileName = "IngredientMaterial", menuName = "Cauldron Chaos/Ingredient Material", order = 0)]
public class IngredientMaterial : ScriptableObject {
  public IngredientMaterial ParentMaterial;
  
  public readonly List<IngredientMaterial> Children = new();
  private IngredientMaterial _lastParent;

  /// <summary>
  /// Returns a list of all the ingredient materials that are affected by this material.
  /// </summary>
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

  /// <summary>
  /// Adds a child to the list of children.
  /// </summary>
  /// <param name="child"></param>
  public void AddChild(IngredientMaterial child) {
    if (!Children.Contains(child)) {
      Children.Add(child);
    }
  }

  public void OnEnable() {
    if (ParentMaterial != null) {
      ParentMaterial.AddChild(this);
    }
  }

  /// <summary>
  /// Called when the script is loaded or a value is changed in the inspector (Called in the editor only).
  /// </summary>
  public void OnValidate() {
    if (ParentMaterial != _lastParent) {
      if (_lastParent != null) {
        _lastParent.Children.Remove(this);
      }

      if (ParentMaterial != null) {
        ParentMaterial.AddChild(this);
      }

      _lastParent = ParentMaterial;
    }
  }
}

