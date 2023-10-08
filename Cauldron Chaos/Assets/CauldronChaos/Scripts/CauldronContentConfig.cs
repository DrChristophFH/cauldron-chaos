using System;

using UnityEngine;

[Serializable]
public class CauldronContentConfig {
  [Header("Colors")]
  public Color BaseColor = new Color(61, 108, 255);
  public Color TopColor = new Color(61, 108, 255);
  public float Shades = 3.0f;

  [Header("Waves")]
  public float WaveStrength = 5.80f;
  public float WaveHeight = 2.5f;
  public float WaveSpeed = 0.05f;
  public float WaveRotation = 180.0f;

  [Header("Bubbles")]
  public float BubbleSpeed = 1.0f;
  public float BubbleDensity = 3.7f;
  public float BubbleSpacing = 0.45f;
  public float BubbleStrength = 0.95f;

  [Header("Circle Size")]
  public float CircleSize = 0.94f;
}