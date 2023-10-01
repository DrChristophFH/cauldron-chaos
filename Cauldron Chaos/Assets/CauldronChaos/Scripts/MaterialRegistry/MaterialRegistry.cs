using System.Collections.Generic;

using UnityEngine;

[CreateAssetMenu(menuName = "Brewing/MaterialRegistry")]
public class MaterialRegistry : ScriptableObject {
  [System.Serializable]
  public class MaterialNode {
    public string Name;
    public string Parent;
    public List<string> Children = new();
  }

  public Dictionary<string, MaterialNode> materials = new Dictionary<string, MaterialNode>();

  public List<string> GetHierarchyPath(string materialName) {
    List<string> path = new List<string>();
    MaterialNode currentNode = materials[materialName];

    while (currentNode != null) {
      path.Insert(0, currentNode.Name);
      if (materials.ContainsKey(currentNode.Parent))
        currentNode = materials[currentNode.Parent];
      else
        currentNode = null;
    }

    return path;
  }
}
