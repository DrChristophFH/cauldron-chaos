using System.Collections.Generic;

using UnityEngine;

public class Cauldron : MonoBehaviour {
  public MaterialRegistry registry;

  private readonly Dictionary<string, int> _materialQuantities = new Dictionary<string, int>();

  public void AddMaterial(string materialName, int quantity) {
    List<string> hierarchyPath = registry.GetHierarchyPath(materialName);

    foreach (string material in hierarchyPath) {
      if (_materialQuantities.ContainsKey(material))
        _materialQuantities[material] += quantity;
      else
        _materialQuantities[material] = quantity;
    }
  }
}
