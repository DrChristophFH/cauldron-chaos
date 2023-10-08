using System;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class Cauldron : MonoBehaviour {
  private readonly Dictionary<IngredientMaterial, int> materials = new();

  [SerializeField]
  private CauldronState state;

  [SerializeField]
  private ParticleSystem smokeParticles;
  [SerializeField]
  private Vector3 offset = new Vector3(0, 0.1f, 0);

  [SerializeField]
  private GameObject transitionParticles;

  [SerializeField]
  private GameObject content;

  private Material liquid;

  public Dictionary<IngredientMaterial, int> Materials => materials;

  private void Start() {
    liquid = content.GetComponent<Renderer>().material;
    // spawn in smoke particles
    smokeParticles = Instantiate(smokeParticles, transform.position + offset, Quaternion.identity);
    ApplySmokeConfig(state.SmokeConfig);
    ApplyContentConfig(state.ContentConfig);
  }

  public void AddPart(Part part) {
    foreach (IngredientMaterial material in part.Material.Affected()) {
      Materials.TryGetValue(material, out int current);
      Materials[material] = current + part.Amount;
    }

    state.CheckTransitions(this);
  }

  public void UpdateMaterials(Dictionary<IngredientMaterial, int> newMaterials) {
    foreach (KeyValuePair<IngredientMaterial, int> pair in newMaterials) {
      Materials[pair.Key] = pair.Value;
    }
  }

  public void TransitionTo(CauldronState destination) {
    Debug.Log($"Transitioning from {state.name} to {destination.name}");
    state = destination;
    PlayTransitionPuff();
    ApplySmokeConfig(destination.SmokeConfig);
    ApplyContentConfig(destination.ContentConfig);
  }

  private void PlayTransitionPuff() {
    var particles = Instantiate(transitionParticles, transform.position + offset, Quaternion.identity);
    Destroy(particles, 5.0f);
  }

  private void ApplySmokeConfig(CauldronSmokeConfig config) {
    var main = smokeParticles.main;
    main.startLifetime = config.StartLifeTime;
    main.startSize = config.StartSize;
    main.maxParticles = config.MaxParticles;
    main.startColor = config.StartColor;
    main.startSpeed = config.StartSpeed;

    var emission = smokeParticles.emission;
    emission.rateOverTime = config.EmissionRate;

    var velocity = smokeParticles.velocityOverLifetime;
    velocity.y = new ParticleSystem.MinMaxCurve(config.VelocityYMin, config.VelocityYMax);

    var color = smokeParticles.colorOverLifetime;
    color.color = config.ColorOverLifetime;

    var noise = smokeParticles.noise;
    noise.strength = config.TurblenceStrength;
    noise.frequency = config.TurblenceFrequency;
    noise.scrollSpeed = config.TurblenceSpeed;

    if (config.Mesh != null) {
      var renderer = smokeParticles.GetComponent<ParticleSystemRenderer>();
      renderer.mesh = config.Mesh;
    }
  }

  private void ApplyContentConfig(CauldronContentConfig config) {
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
  }
}