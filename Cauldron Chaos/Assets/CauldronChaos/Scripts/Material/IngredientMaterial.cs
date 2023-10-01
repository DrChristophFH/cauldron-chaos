using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brewing/IngredientMaterial")]
public class IngredientMaterial : ScriptableObject {
  [SerializeField]
  private string nodeName;

  [System.NonSerialized]
  public IngredientMaterial parentNode;

  public List<IngredientMaterial> children = new();
}