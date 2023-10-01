using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(IngredientMaterial)), CanEditMultipleObjects]
public class IngredientMaterialEditor : Editor {

  public override void OnInspectorGUI() {
    // Draw default inspector, but you might skip drawing the Children list for now.
    DrawDefaultInspector();

    // Display Children list as read-only
    IngredientMaterial material = (IngredientMaterial)target;

    EditorGUILayout.LabelField("Children", EditorStyles.boldLabel);
    EditorGUI.BeginDisabledGroup(true); // Disable editing
    foreach (IngredientMaterial child in material.Children) {
      EditorGUILayout.ObjectField(child, typeof(IngredientMaterial), allowSceneObjects: false);
    }
    EditorGUI.EndDisabledGroup(); // End disabling
  }
}