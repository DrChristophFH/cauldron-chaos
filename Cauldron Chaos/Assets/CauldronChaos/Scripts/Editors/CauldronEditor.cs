using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(Cauldron))]
public class CauldronEditor : Editor {

  public override void OnInspectorGUI() {
    // Draw default inspector, but you might skip drawing the Children list for now.
    DrawDefaultInspector();

    // Display Children list as read-only
    Dictionary<IngredientMaterial, int> materials = ((Cauldron) target).Materials;

    EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);
    foreach (KeyValuePair<IngredientMaterial, int> material in materials) {
      EditorGUILayout.LabelField($"{material.Key.name}: {material.Value}");
    }
  }
}