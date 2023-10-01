using System.Collections.Generic;

using UnityEngine;

public class Cauldron : MonoBehaviour {
    private readonly Dictionary<IngredientMaterial, int> materials = new();

    public Dictionary<IngredientMaterial, int> Materials => materials;

    public void AddPart(Part part) {
    foreach (IngredientMaterial material in part.Material.Affected()) {
      Materials.TryGetValue(material, out int current);
      Materials[material] = current + part.Amount;
    }
  }
}