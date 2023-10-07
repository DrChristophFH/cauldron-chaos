using UnityEngine;

[CreateAssetMenu(fileName = "CauldronSmokeConfig", menuName = "Cauldron Chaos/Smoke Config", order = 1)]
public class CauldronSmokeConfig : ScriptableObject {
  [Header("Main Config")]
  public float StartLifeTime = 4.0f;
  public float StartSize = 0.3f;
  public int MaxParticles = 100;

  [Header("Emission Config")]
  public float EmissionRate = 5.0f;

  [Header("Velocity Config")]
  public float StartSpeed = 0.05f;
  public float VelocityYMax = 0.6f;
  public float VelocityYMin = 0.5f;

  [Header("Color Config")]
  public Color StartColor = new Color(61, 108, 255);
  public Gradient ColorOverLifetime = new();

  [Header("Turblence Config")]
  public float TurblenceStrength = 0.01f;
  public float TurblenceFrequency = 0.2f;
  public float TurblenceSpeed = 0.2f;

  [Header("Renderer Config")]
  public Mesh Mesh;
}
