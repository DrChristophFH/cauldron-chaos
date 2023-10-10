using UnityEngine;
using UnityEditor;

public class ContentPreviewer : EditorWindow {

  private CauldronState selectedState;

  [MenuItem("Tools/Content Previewer")]
  public static void ShowWindow() {
    GetWindow<ContentPreviewer>("Content Previewer");
  }

  private void OnGUI() {
    GUILayout.Label("Select a CauldronState", EditorStyles.boldLabel);

    selectedState = (CauldronState)EditorGUILayout.ObjectField("Cauldron State", selectedState, typeof(CauldronState), false);

    if (GUILayout.Button("Preview Content")) {
      PreviewContent();
    }
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
