using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class WreckingBallThrower : MonoBehaviour {
  [SerializeField]
  private Vector3 force = new Vector3(100, 0, 0);

  void Update() {
    if (Input.GetKeyDown(KeyCode.F)) {
      GetComponent<Rigidbody>().AddForce(force);
    }
  }
}
