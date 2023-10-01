using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

using UnityEngine;

public class MaterialRegistryTreeView : TreeView {
    readonly MaterialRegistry _registry;

  public MaterialRegistryTreeView(TreeViewState state, MaterialRegistry registry) : base(state) {
    _registry = registry;
    Reload();
  }

  protected override TreeViewItem BuildRoot() {
    // The root item is required but not actually displayed
    MaterialRegistryItem root = new MaterialRegistryItem { id = -1, depth = -1 };
    var allItems = new List<MaterialRegistryItem>();

    // Convert the dictionary materials into tree items
    foreach (var material in _registry.materials) {
      var item = new MaterialRegistryItem {
        id = material.Key.GetHashCode(),
        materialName = material.Key,
        materialNode = material.Value
      };

      allItems.Add(item);
    }

    // Set children for each item
    foreach (var item in allItems) {
      if (item.materialNode.Parent == null) {
        root.AddChild(item);
      } 
      
      List<MaterialRegistryItem> children = allItems.FindAll(x => x.materialNode.Parent == item.materialName);
      foreach (var child in children) {
        item.AddChild(child);
      }
    }

    return root;
  }
}
