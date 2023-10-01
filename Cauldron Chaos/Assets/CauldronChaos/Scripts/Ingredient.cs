using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Ingredient : MonoBehaviour {

  [SerializeField]
  private string ingredientName;
  
  void OnTriggerEnter(Collider other) {
    if (other.gameObject.CompareTag("Cauldron")) {
      Debug.Log("Ingredient " + ingredientName + " entered cauldron");
      GetComponent<Rigidbody>().AddForce(100, 400, 0);
    }
  }
}
