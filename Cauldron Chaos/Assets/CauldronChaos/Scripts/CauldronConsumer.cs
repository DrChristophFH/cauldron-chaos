using UnityEngine;

public class CauldronConsumer : MonoBehaviour {
  [SerializeField]
  private Cauldron cauldron;

  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent(out Ingredient ingredient)) {
      foreach (Part part in ingredient.Parts) {
        Debug.Log($" - Adding {part.Amount} {part.Material.name} to cauldron from {ingredient.Name}");
        cauldron.AddPart(part);
      }
    }
  }
}