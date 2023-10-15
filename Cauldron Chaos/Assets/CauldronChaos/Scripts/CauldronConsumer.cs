using UnityEngine;

public class CauldronConsumer : MonoBehaviour {
  [SerializeField]
  private Cauldron cauldron;
  [SerializeField]
  private GameObject dropParticles;
  [SerializeField]
  private AudioClip dropSound;

  private void OnTriggerEnter(Collider other) {
    if (other.TryGetComponent(out Ingredient ingredient)) {
      AddIngredients(ingredient);
      var position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
      var drop = Instantiate(dropParticles, position, Quaternion.identity);
      AudioHelper.PlayClipAtPointWithSettings(dropSound, position);
      Destroy(drop, 2f);
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