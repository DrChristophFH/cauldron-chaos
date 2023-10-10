using UnityEngine;
using UnityEditor;

public class StateInitializer : EditorWindow {

  private float shadesMin = 3;
  private float shadesMax = 7;
  private float waveStrengthMin = 3.0f;
  private float waveStrengthMax = 18.0f;
  private float waveHeightMin = 0.1f;
  private float waveHeightMax = 1.0f;
  private float waveSpeedMin = 0.0f;
  private float waveSpeedMax = 1.5f;
  private float waveRotationMin = 0.0f;
  private float waveRotationMax = 360f;
  private float bubbleSpeedMin = 0.5f;
  private float bubbleSpeedMax = 2.0f;
  private float bubbleDensityMin = 0.6f;
  private float bubbleDensityMax = 6.0f;
  private float bubbleSpacingMin = 0.05f;
  private float bubbleSpacingMax = 0.5f;
  private float bubbleStrengthMin = 0.7f;
  private float bubbleStrengthMax = 1.3f;

  private bool setNoOverwrite = false;

  [MenuItem("Tools/State Initializer")]
  public static void ShowWindow() {
    GetWindow<StateInitializer>("Cauldron State Initializer");
  }

  private void OnGUI() {
    GUILayout.Label("Configuration for Cauldron States", EditorStyles.boldLabel);

    // Input fields for configuration
    HorizontalGroup("Shades", ref shadesMin, ref shadesMax);
    HorizontalGroup("Wave Strength", ref waveStrengthMin, ref waveStrengthMax);
    HorizontalGroup("Wave Height", ref waveHeightMin, ref waveHeightMax);
    HorizontalGroup("Wave Speed", ref waveSpeedMin, ref waveSpeedMax);
    HorizontalGroup("Wave Rotation", ref waveRotationMin, ref waveRotationMax);
    HorizontalGroup("Bubble Speed", ref bubbleSpeedMin, ref bubbleSpeedMax);
    HorizontalGroup("Bubble Density", ref bubbleDensityMin, ref bubbleDensityMax);
    HorizontalGroup("Bubble Spacing", ref bubbleSpacingMin, ref bubbleSpacingMax);
    HorizontalGroup("Bubble Strength", ref bubbleStrengthMin, ref bubbleStrengthMax);

    EditorGUILayout.Space();
    setNoOverwrite = EditorGUILayout.Toggle("Set No Overwrite", setNoOverwrite);

    if (GUILayout.Button("Update Cauldron States")) {
      UpdateCauldronStates();
    }
  }

  private void HorizontalGroup(string title, ref float min, ref float max) {
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField(title, GUILayout.Width(100));
    min = EditorGUILayout.FloatField(min, GUILayout.Width(50));
    max = EditorGUILayout.FloatField(max, GUILayout.Width(50));
    EditorGUILayout.EndHorizontal();
  }

  private void UpdateCauldronStates() {
    //get all CauldronState assets
    string[] guids = AssetDatabase.FindAssets("t:CauldronState");
    foreach (string guid in guids) {
      string path = AssetDatabase.GUIDToAssetPath(guid);
      CauldronState state = AssetDatabase.LoadAssetAtPath<CauldronState>(path);
      UpdateCauldronState(state);
    }
  }

  private void UpdateCauldronState(CauldronState state) {
    if (state.NoOverwrite) {
      return;
    }
    CauldronContentConfig config = state.ContentConfig;
    config.BaseColor = Random.ColorHSV(0, 1, 0.5f, 1, 0.5f, 1);
    config.TopColor = new Color(config.BaseColor.r * 0.8f, config.BaseColor.g * 0.8f, config.BaseColor.b * 0.8f);
    config.Shades = (int)Random.Range(shadesMin, shadesMax);
    config.WaveStrength = Random.Range(waveStrengthMin, waveStrengthMax);
    config.WaveHeight = Random.Range(waveHeightMin, waveHeightMax);
    config.WaveSpeed = Random.Range(waveSpeedMin, waveSpeedMax);
    config.WaveRotation = Random.Range(waveRotationMin, waveRotationMax);
    config.BubbleSpeed = Random.Range(bubbleSpeedMin, bubbleSpeedMax);
    config.BubbleDensity = Random.Range(bubbleDensityMin, bubbleDensityMax);
    config.BubbleSpacing = Random.Range(bubbleSpacingMin, bubbleSpacingMax);
    config.BubbleStrength = Random.Range(bubbleStrengthMin, bubbleStrengthMax);
    state.NoOverwrite = setNoOverwrite;
  }
}