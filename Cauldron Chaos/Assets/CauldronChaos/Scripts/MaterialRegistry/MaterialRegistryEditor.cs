using UnityEditor;
using UnityEditor.IMGUI.Controls;

using UnityEngine;

[CustomEditor(typeof(MaterialRegistry))]
public class MaterialRegistryEditor : Editor {
  private MaterialRegistryTreeView _treeView;
  private TreeViewState _treeViewState;

  private void OnEnable() {
    if (_treeViewState == null)
      _treeViewState = new TreeViewState();

    _treeView = new MaterialRegistryTreeView(_treeViewState, (MaterialRegistry)target);
  }

  public override void OnInspectorGUI() {
    _treeView.OnGUI(new Rect(0, 0, Screen.width, Screen.height));
  }
}
