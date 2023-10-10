using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class StateGraphParser : Editor
{
  [MenuItem("Tools/Parse Dot File")]
  public static void ParseDotFile()
  {
    string path = EditorUtility.OpenFilePanel("Open .dot File", "", "dot");
    if (string.IsNullOrEmpty(path))
      return;

    string content = File.ReadAllText(path);
    ParseStates(content);
    ParseTransitions(content);
  }

  /// <summary>
  /// Parses .dot states from the given content using a regular expression and creates a CauldronState asset for each state found.
  /// </summary>
  /// <param name="content">The content to parse.</param>
  private static void ParseStates(string content)
  {
    // Regular expression to extract node definitions
    var matches = Regex.Matches(content, @"(\w+)\s*\[.*?\];");
    foreach (Match match in matches)
    {
      string stateName = match.Groups[1].Value;
      CauldronState state = CreateStateAsset(stateName);
    }
  }

  /// <summary>
  /// Parses .dot transitions from the given content, that have a label, and adds them to the corresponding states.
  /// </summary>
  /// <param name="content">The content to parse.</param>
  private static void ParseTransitions(string content)
  {
    // Regular expression to extract edge definitions
    var matches = Regex.Matches(content, @"(\w+)\s*->\s*(\w+).*?label=""(.*?)"";");
    foreach (Match match in matches)
    {
      string fromStateName = match.Groups[1].Value;
      string toStateName = match.Groups[2].Value;
      string label = match.Groups[3].Value;

      CauldronState fromState = AssetDatabase.LoadAssetAtPath<CauldronState>("Assets/CauldronChaos/Data/States/" + fromStateName + ".asset");
      CauldronState toState = AssetDatabase.LoadAssetAtPath<CauldronState>("Assets/CauldronChaos/Data/States/" + toStateName + ".asset");

      List<Part> parts = GetParts(label);

      fromState.AddTransition(
        new CauldronTransition(toState, )
      );
    }
  }

  /// <summary>
  /// Retrieves a list of parts based on the given label. Parts are formatted as follows:
  /// <Number> <Material>
  /// Seperated by a newline.
  /// </summary>
  /// <param name="label">The label text</param>
  /// <returns>A list of parts.</returns>
  private static List<Part> GetParts(string label)
  {
    List<Part> parts = new List<Part>();
    string[] lines = label.Split('\n');
    foreach (string line in lines)
    {
      string[] parts = line.Split(' ');
      int amount = int.Parse(parts[0]);
      IngredientMaterial material = AssetDatabase.LoadAssetAtPath<IngredientMaterial>("Assets/CauldronChaos/Data/Materials/" + parts[1] + ".asset");
      parts.Add(new Part(amount, material));
    }
    return parts;
  }

  private static CauldronState CreateStateAsset(string stateName)
  {
    CauldronState state = ScriptableObject.CreateInstance<CauldronState>();
    AssetDatabase.CreateAsset(state, "Assets/CauldronChaos/Data/States/" + stateName + ".asset");
    return state;
  }
}
