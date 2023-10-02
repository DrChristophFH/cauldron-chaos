using System.Collections;
using System.Collections.Generic;

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
}
