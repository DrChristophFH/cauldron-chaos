using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

public class StateGraphParser : Editor {
  /// <summary>
  /// Parses a .dot file and extracts the states and transitions from it.
  /// </summary>
  [MenuItem("Tools/Cauldron State Dot File Parser")]
  public static void ParseDotFile() {
    string path = EditorUtility.OpenFilePanel("Open .dot File", "", "dot");
    if (string.IsNullOrEmpty(path))
      return;

    string content = File.ReadAllText(path);

    //strip comments
    content = Regex.Replace(content, @"//.*", "");

    bool keepExistingTransitions = EditorUtility.DisplayDialog("Keep Existing Transitions?", "Do you want to keep existing transitions in already known states?", "Yes", "No");

    ParseStates(content, keepExistingTransitions);
    ParseTransitions(content);
    CleanUp();
  }

  /// <summary>
  /// Cleans up the project by removing the states "graph", "node" and "edge" that can be created by the .dot parser.
  /// </summary>
  private static void CleanUp() {
    AssetDatabase.DeleteAsset("Assets/CauldronChaos/Data/States/graph.asset");
    AssetDatabase.DeleteAsset("Assets/CauldronChaos/Data/States/node.asset");
    AssetDatabase.DeleteAsset("Assets/CauldronChaos/Data/States/edge.asset");
  }

  /// <summary>
  /// Parses .dot states from the given content using a regular expression and creates a CauldronState asset for each state found.
  /// </summary>
  /// <param name="content">The content to parse.</param>
  private static void ParseStates(string content, bool keepExistingTransitions) {
    // Regular expression to extract node definitions
    var matches = Regex.Matches(content, @"(?:\n *)(\w+)\s*\[.*?\];");
    Debug.Log("Found " + matches.Count + " states.");
    foreach (Match match in matches) {
      string stateName = match.Groups[1].Value;
      CauldronState state = CreateStateAsset(stateName);
      state.ClearTransitions();
    }
  }

  /// <summary>
  /// Parses .dot transitions from the given content, that have a label, and adds them to the corresponding states.
  /// </summary>
  /// <param name="content">The content to parse.</param>
  private static void ParseTransitions(string content) {
    // Regular expression to extract edge definitions
    var matches = Regex.Matches(content, @"(\w+)\s*->\s*(\w+).*?label ?= ?""(.*?)"";");
    Debug.Log("Found " + matches.Count + " transitions.");
    foreach (Match match in matches) {
      string fromStateName = match.Groups[1].Value;
      string toStateName = match.Groups[2].Value;
      string label = match.Groups[3].Value;

      CauldronState fromState = AssetDatabase.LoadAssetAtPath<CauldronState>($"Assets/CauldronChaos/Data/States/{fromStateName}.asset");
      CauldronState toState = AssetDatabase.LoadAssetAtPath<CauldronState>($"Assets/CauldronChaos/Data/States/{toStateName}.asset");

      List<Part> parts = GetParts(label);

      fromState.AddTransition(
        new CauldronTransition(toState, parts)
      );
    }
  }

  /// <summary>
  /// Retrieves a list of parts based on the given label.
  /// </summary>
  /// <remarks> 
  /// Parts need to be formatted <c>[Amount] [Material]</c> and seperated by a new line.
  /// </remarks>
  /// <param name="label">The label text</param>
  /// <returns>A list of parts.</returns>
  private static List<Part> GetParts(string label) {
    List<Part> parts = new List<Part>();
    string[] lines = label.Split('\n');
    foreach (string line in lines) {
      string[] chunk = line.Split(' ');
      try {
        int amount = int.Parse(chunk[0]);
        IngredientMaterial material = AssetDatabase.LoadAssetAtPath<IngredientMaterial>($"Assets/CauldronChaos/Data/Materials/{parts[1]}.asset");
        parts.Add(new Part { Amount = amount, Material = material });
      } catch (Exception) {
        Debug.LogWarning($"Could not parse part from line {line}. Format should be <Number> <Material>");
      }
    }
    return parts;
  }

  /// <summary>
  /// Creates a new instance of a CauldronState asset with the given state name and saves it to the project.
  /// </summary>
  /// <param name="stateName">The name of the state to create.</param>
  /// <returns>The newly created CauldronState asset.</returns>
  private static CauldronState CreateStateAsset(string stateName) {
    // try to load the asset first, if it exists, return it
    CauldronState existingState = AssetDatabase.LoadAssetAtPath<CauldronState>($"Assets/CauldronChaos/Data/States/{stateName}.asset");
    if (existingState != null) {
      Debug.Log($"State {stateName} already exists, skipping creation.");
      return existingState;
    }

    CauldronState state = CreateInstance<CauldronState>();
    AssetDatabase.CreateAsset(state, $"Assets/CauldronChaos/Data/States/{stateName}.asset");
    return state;
  }
}
