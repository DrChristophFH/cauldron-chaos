using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CauldronState))]
public class CauldronStateEditor : Editor
{
  private SerializedProperty smokeConfigProperty;

  private void OnEnable()
  {
    smokeConfigProperty = serializedObject.FindProperty("smokeConfig");
  }

  public override void OnInspectorGUI()
  {
    serializedObject.Update();

    // Call the base implementation to draw the other editor elements
    base.OnInspectorGUI();

    // Show the editor for the SmokeConfig if it's not null
    if (smokeConfigProperty.objectReferenceValue != null)
    {
      EditorGUI.indentLevel++;
      Editor smokeConfigEditor = CreateEditor(smokeConfigProperty.objectReferenceValue);
      smokeConfigEditor.OnInspectorGUI();
      EditorGUI.indentLevel--;
    }

    serializedObject.ApplyModifiedProperties();
  }
}
