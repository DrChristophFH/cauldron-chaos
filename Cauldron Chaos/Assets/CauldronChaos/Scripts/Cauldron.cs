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

  public Dictionary<IngredientMaterial, int> Materials => materials;

  private void Start() {
    // spawn in smoke particles
    smokeParticles = Instantiate(smokeParticles, transform.position, Quaternion.identity);
    ApplySmokeConfig(state.SmokeConfig);
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
    ApplySmokeConfig(destination.SmokeConfig);
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
}