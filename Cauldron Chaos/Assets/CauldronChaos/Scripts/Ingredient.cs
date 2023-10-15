using System.Collections;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Throwable))]
public class Ingredient : MonoBehaviour {
  [SerializeField]
  private string ingredientName;
  [SerializeField]
  private List<Part> parts = new();

  public string Name => ingredientName;
  public List<Part> Parts => parts;

  public string GetDescription() {
    StringBuilder description = new StringBuilder();
    description.AppendLine($"{ingredientName}:");
    foreach (Part part in parts) {
      description.AppendLine($"{part.Amount} {part.Material}");
    }
    return description.ToString();
  }
}
