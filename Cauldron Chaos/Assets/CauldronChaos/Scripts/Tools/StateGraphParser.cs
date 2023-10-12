using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using State = DotParser.State;
using Transition = DotParser.Transition;

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
      if (!keepExistingTransitions) {
        asset.ClearTransitions();
      }
    }
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
    }
  }

  private static List<Part> GetParts(string label) {
    List<Part> parts = new List<Part>();
    string[] lines = label.Split('\n');
    foreach (string line in lines) {
      string[] chunk = line.Split(' ');
      try {
        int amount = int.Parse(chunk[0]);
        IngredientMaterial material = AssetDatabase.LoadAssetAtPath<IngredientMaterial>($"{MATERIALS_PATH}{parts[1]}.asset");
        parts.Add(new Part { Amount = amount, Material = material });
      } catch (Exception) {
        Debug.LogWarning($"Could not parse part from line {line}. Format should be <Number> <Material>");
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
