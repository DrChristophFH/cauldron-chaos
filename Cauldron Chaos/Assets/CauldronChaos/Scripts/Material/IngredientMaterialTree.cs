using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Brewing/IngredientMaterialTree")]
public class IngredientMaterialTree : ScriptableObject {
  [SerializeField]
  private IngredientMaterial root;
}