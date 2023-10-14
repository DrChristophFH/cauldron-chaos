using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using System;

public class ContentPreviewer : EditorWindow {

  private string[] states;
  private int stateIndex = 0;
  private CauldronState selectedState;

  [MenuItem("Tools/Content Previewer")]
  public static void ShowWindow() {
    GetWindow<ContentPreviewer>("Content Previewer");
  }

  private void Awake() {
    RefreshStates();
  }

  private void RefreshStates() {
    states = AssetDatabase.FindAssets("t:CauldronState");
    states = Array.ConvertAll(states, guid => AssetDatabase.GUIDToAssetPath(guid));
    selectedState = AssetDatabase.LoadAssetAtPath<CauldronState>(states[stateIndex]);
  }

  private void OnGUI() {
    GUILayout.Label("Select a CauldronState", EditorStyles.boldLabel);

    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("CauldronState", GUILayout.Width(100));

    if (GUILayout.Button("<-")) {
      stateIndex--;
      if (stateIndex < 0) {
        stateIndex = states.Length - 1;
      }
    }

    // dropdown menu for selecting a CauldronState
    stateIndex = EditorGUILayout.Popup(stateIndex, Array.ConvertAll(states, path => path.Substring(path.LastIndexOf('/') + 1)), GUILayout.Width(200));
    selectedState = AssetDatabase.LoadAssetAtPath<CauldronState>(states[stateIndex]);

    if (GUILayout.Button("->")) {
      stateIndex++;
      if (stateIndex >= states.Length) {
        stateIndex = 0;
      }
    }
    EditorGUILayout.EndHorizontal();

    if (GUILayout.Button("Refresh")) {
      RefreshStates();
    }

    if (GUILayout.Button("Preview Content")) {
      PreviewContent();
    }

    if (GUILayout.Button("Save Content to State")) {
      SaveContentToState();
    }
  }

  private void SaveContentToState() {
    // Find the GameObject named "cauldron" in the scene
    GameObject cauldronObj = GameObject.Find("CaudlronContent");

    if (cauldronObj != null) {
      Material liquid = cauldronObj.GetComponent<MeshRenderer>().sharedMaterial;

      if (!CheckNoOverwrite(selectedState.NoOverwrite)) {
        return;
      }

      CauldronContentConfig config = selectedState.ContentConfig;
      config.BaseColor = liquid.GetColor("_Base_Color");
      config.TopColor = liquid.GetColor("_Top_Color");
      config.Shades = (int)liquid.GetFloat("_Shades");
      config.WaveStrength = liquid.GetFloat("_Wave_Strength");
      config.WaveHeight = liquid.GetFloat("_Wave_Height");
      config.WaveSpeed = liquid.GetFloat("_Wave_Speed");
      config.WaveRotation = liquid.GetFloat("_Wave_Rotation");
      config.BubbleSpeed = liquid.GetFloat("_Bubble_Speed");
      config.BubbleDensity = liquid.GetFloat("_Bubble_Density");
      config.BubbleSpacing = liquid.GetFloat("_Bubble_Spacing");
      config.BubbleStrength = liquid.GetFloat("_Bubble_Strength");
      config.CircleSize = liquid.GetFloat("_Circle_Size");
      selectedState.NoOverwrite = true;
      EditorUtility.SetDirty(selectedState);
    } else {
      Debug.LogError("GameObject named 'cauldron' not found in the scene!");
    }
  }

  private bool CheckNoOverwrite(bool noOverwrite) {
    if (noOverwrite) {
      if (!EditorUtility.DisplayDialog("Overwrite Warning", "This CauldronState has the 'No Overwrite' flag set. Are you sure you want to overwrite it?", "Yes", "No")) {
        Debug.Log("Save cancelled");
        return false;
      }
    }
    return true;
  }

  private void PreviewContent() {
    // Find the GameObject named "cauldron" in the scene
    GameObject cauldronObj = GameObject.Find("CaudlronContent");

    if (cauldronObj != null) {
      Material liquid = cauldronObj.GetComponent<MeshRenderer>().sharedMaterial;
      CauldronContentConfig config = selectedState.ContentConfig;
      liquid.SetColor("_Base_Color", config.BaseColor);
      liquid.SetColor("_Top_Color", config.TopColor);
      liquid.SetFloat("_Shades", config.Shades);
      liquid.SetFloat("_Wave_Strength", config.WaveStrength);
      liquid.SetFloat("_Wave_Height", config.WaveHeight);
      liquid.SetFloat("_Wave_Speed", config.WaveSpeed);
      liquid.SetFloat("_Wave_Rotation", config.WaveRotation);
      liquid.SetFloat("_Bubble_Speed", config.BubbleSpeed);
      liquid.SetFloat("_Bubble_Density", config.BubbleDensity);
      liquid.SetFloat("_Bubble_Spacing", config.BubbleSpacing);
      liquid.SetFloat("_Bubble_Strength", config.BubbleStrength);
      liquid.SetFloat("_Circle_Size", config.CircleSize);
    } else {
      Debug.LogError("GameObject named 'cauldron' not found in the scene!");
    }
  }
}
