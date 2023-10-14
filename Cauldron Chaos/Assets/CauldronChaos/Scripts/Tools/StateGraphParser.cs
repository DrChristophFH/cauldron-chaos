using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using State = DotParser.State;
using Transition = DotParser.Transition;
using UnityEditor.VersionControl;

public class StateGraphParser : Editor {

  private static readonly DotParser parser = new DotParser();
  private static readonly string STATES_PATH = "Assets/CauldronChaos/Data/States/";
  private static readonly string MATERIALS_PATH = "Assets/CauldronChaos/Data/Materials/";

  [MenuItem("Tools/Cauldron State Dot File Parser")]
  public static void ParseDotFile() {
    string path = EditorUtility.OpenFilePanel("Open .dot File", "", "dot");
    if (string.IsNullOrEmpty(path))
      return;

    DotParser.Graph graph = parser.Parse(path);

    bool keepExistingTransitions = EditorUtility.DisplayDialog("Keep Existing Transitions?", "Do you want to keep existing transitions in already known states?", "Yes", "No");

    HandleStates(graph.States, keepExistingTransitions);
    CheckExistingStates(graph.States);
    HandleTransitions(graph.Transitions);
  }

  private static void HandleStates(List<State> states, bool keepExistingTransitions) {
    Debug.Log("Found " + states.Count + " states.");

    foreach (State state in states) {
      CauldronState asset = TryGetStateAsset(state.Name);
      ConfigureStateVisuals(asset, state.Properties);
      if (!keepExistingTransitions) {
        asset.ClearTransitions();
      }
    }
  }

  private static void ConfigureStateVisuals(CauldronState state, Dictionary<string, string> properties) {
    CauldronContentConfig config = state.ContentConfig;
    config.BaseColor = TryGetColor(properties["BaseColor"], out Color color) ? color : config.BaseColor;
    config.TopColor = TryGetColor(properties["TopColor"], out color) ? color : config.TopColor;
    config.Shades = int.Parse(properties["Shades"]);
    config.WaveStrength = float.Parse(properties["WaveStrength"]);
    config.WaveHeight = float.Parse(properties["WaveHeight"]);
    config.WaveSpeed = float.Parse(properties["WaveSpeed"]);
    config.WaveRotation = float.Parse(properties["WaveRotation"]);
    config.BubbleSpeed = float.Parse(properties["BubbleSpeed"]);
    config.BubbleDensity = float.Parse(properties["BubbleDensity"]);
    config.BubbleSpacing = float.Parse(properties["BubbleSpacing"]);
    config.BubbleStrength = float.Parse(properties["BubbleStrength"]);

    CauldronSmokeConfig smokeConfig = state.SmokeConfig;
    smokeConfig.StartLifeTime = float.Parse(properties["StartLifeTime"]);
    smokeConfig.StartSize = float.Parse(properties["StartSize"]);
    smokeConfig.MaxParticles = int.Parse(properties["MaxParticles"]);
    smokeConfig.StartColor = TryGetColor(properties["StartColor"], out color) ? color : state.SmokeConfig.StartColor;
    smokeConfig.StartSpeed = float.Parse(properties["StartSpeed"]);
    smokeConfig.EmissionRate = float.Parse(properties["EmissionRate"]);
    smokeConfig.VelocityYMin = float.Parse(properties["VelocityYMin"]);
    smokeConfig.VelocityYMax = float.Parse(properties["VelocityYMax"]);
    smokeConfig.TurblenceStrength = float.Parse(properties["TurblenceStrength"]);
    smokeConfig.TurblenceFrequency = float.Parse(properties["TurblenceFrequency"]);
    smokeConfig.TurblenceSpeed = float.Parse(properties["TurblenceSpeed"]);
    
    state.NoOverwrite = true;
  }

  private static bool TryGetColor(string colorString, out Color color) {
    string[] rgba = colorString.Split(',');
    if (rgba.Length != 4) {
      Debug.LogWarning($"Could not parse color from {colorString}. Format should be <R>,<G>,<B>");
      color = Color.white;
      return false;
    }

    try {
      color = new Color(float.Parse(rgba[0]), float.Parse(rgba[1]), float.Parse(rgba[2]), float.Parse(rgba[3]));
    } catch (Exception) {
      Debug.LogWarning($"Could not parse color from {colorString}. Format should be <R>,<G>,<B>, <A>");
      color = Color.white;
      return false;
    }

    return true;
  }

  private static void CheckExistingStates(List<State> states) {
    List<string> statesNotInDotFile = new();

    foreach (string state in AssetDatabase.FindAssets("t:CauldronState", new[] { "Assets/CauldronChaos/Data/States" })) {
      string stateName = Path.GetFileNameWithoutExtension(AssetDatabase.GUIDToAssetPath(state));
      if (!states.Exists(s => s.Name == stateName)) {
        statesNotInDotFile.Add(stateName);
      }
    }

    if (statesNotInDotFile.Count == 0) {
      return;
    }

    bool deleteStates = EditorUtility.DisplayDialog("Delete States?", $"Do you want to delete the following states that are not in the .dot file?\n{string.Join("\n", statesNotInDotFile)}", "Yes", "No");
    if (deleteStates) {
      foreach (string stateName in statesNotInDotFile) {
        AssetDatabase.DeleteAsset($"{STATES_PATH}{stateName}.asset");
      }
    }
  }

  private static void HandleTransitions(List<Transition> transitions) {
    Debug.Log("Found " + transitions.Count + " transitions.");

    foreach (Transition transition in transitions) {
      CauldronState fromState = AssetDatabase.LoadAssetAtPath<CauldronState>($"{STATES_PATH}{transition.Source}.asset");
      CauldronState toState = AssetDatabase.LoadAssetAtPath<CauldronState>($"{STATES_PATH}{transition.Destination}.asset");

      List<Part> parts = GetParts(transition.Properties["label"]);

      fromState.AddTransition(
        new CauldronTransition(toState, parts)
      );

      EditorUtility.SetDirty(fromState);
    }
    AssetDatabase.SaveAssets();
  }

  private static List<Part> GetParts(string label) {
    List<Part> parts = new List<Part>();
    string[] lines = label.Split("\\n"); // necessary because of the way the .dot file is parsed
    foreach (string line in lines) {
      string[] chunk = line.Split(' ');
      try {
        int amount = int.Parse(chunk[0]);
        IngredientMaterial material = AssetDatabase.LoadAssetAtPath<IngredientMaterial>($"{MATERIALS_PATH}{chunk[1]}.asset");
        parts.Add(new Part { Amount = amount, Material = material });
      } catch (Exception e) {
        Debug.LogWarning($"Could not parse part from line {line}. Format should be <Number> <Material>. Error: {e.Message}");
      }
    }
    return parts;
  }

  private static CauldronState TryGetStateAsset(string stateName) {
    // try to load the asset first, if it exists, return it
    CauldronState existingState = AssetDatabase.LoadAssetAtPath<CauldronState>($"{STATES_PATH}{stateName}.asset");
    if (existingState != null) {
      Debug.Log($"State {stateName} already exists, skipping creation.");
      return existingState;
    }

    CauldronState state = CreateInstance<CauldronState>();
    AssetDatabase.CreateAsset(state, $"{STATES_PATH}{stateName}.asset");
    return state;
  }
}
