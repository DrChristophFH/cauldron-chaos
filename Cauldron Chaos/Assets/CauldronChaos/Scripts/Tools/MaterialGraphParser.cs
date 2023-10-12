using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using State = DotParser.State;
using Transition = DotParser.Transition;

public class MaterialGraphParser : Editor {

  private static readonly DotParser parser = new DotParser();
  private static readonly string MATERIALS_PATH = "Assets/CauldronChaos/Data/Materials/";

  [MenuItem("Tools/Material Dot File Parser")]
  public static void ParseDotFile() {
    string path = EditorUtility.OpenFilePanel("Open .dot File", "", "dot");
    if (string.IsNullOrEmpty(path))
      return;

    DotParser.Graph graph = parser.Parse(path);

    HandleStates(graph.States);
    HandleTransitions(graph.Transitions);
  }

  private static void HandleStates(List<State> materials) {
    Debug.Log("Found " + materials.Count + " materials.");

    foreach (State material in materials) {
      IngredientMaterial asset = TryGetStateAsset(material.Name);
      asset.ClearChildren();
    }
  }

  private static void HandleTransitions(List<Transition> transitions) {
    Debug.Log("Found " + transitions.Count + " transitions.");

    foreach (Transition transition in transitions) {
      IngredientMaterial fromState = AssetDatabase.LoadAssetAtPath<IngredientMaterial>($"{MATERIALS_PATH}{transition.Source}.asset");
      IngredientMaterial toState = AssetDatabase.LoadAssetAtPath<IngredientMaterial>($"{MATERIALS_PATH}{transition.Destination}.asset");

      fromState.AddChild(toState);
    }
  }

  private static IngredientMaterial TryGetStateAsset(string materialName) {
    // try to load the asset first, if it exists, return it
    IngredientMaterial existingMaterial = AssetDatabase.LoadAssetAtPath<IngredientMaterial>($"{MATERIALS_PATH}{materialName}.asset");
    if (existingMaterial != null) {
      Debug.Log($"Material {materialName} already exists, skipping creation.");
      return existingMaterial;
    }

    IngredientMaterial material = CreateInstance<IngredientMaterial>();
    AssetDatabase.CreateAsset(material, $"{MATERIALS_PATH}{materialName}.asset");
    return material;
  }
}
