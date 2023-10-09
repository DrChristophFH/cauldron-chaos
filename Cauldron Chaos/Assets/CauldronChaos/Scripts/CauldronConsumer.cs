using UnityEngine;

public class CauldronConsumer : MonoBehaviour {
  [SerializeField]
  private Cauldron cauldron;
  [SerializeField]
  private GameObject dropParticles;

  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent(out Ingredient ingredient)) {
      AddIngredients(ingredient);
      var position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
      Instantiate(dropParticles, position, Quaternion.identity);
      Destroy(other.gameObject);
    }
  }

  private void AddIngredients(Ingredient ingredient) {
    foreach (Part part in ingredient.Parts) {
      Debug.Log($" - Adding {part.Amount} {part.Material.name} to cauldron from {ingredient.Name}");
      cauldron.AddPart(part);
    }
  }
}