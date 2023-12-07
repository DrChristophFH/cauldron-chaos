using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class FacePlayer : MonoBehaviour {

  [SerializeField]
  private Transform target;

  void Update() {
    Vector3 directionToTarget = target.position - transform.position;
    directionToTarget.y = 0; // Ignore the y-axis to only rotate on the Y plane

    Quaternion rotation = Quaternion.LookRotation(directionToTarget);
    transform.rotation = Quaternion.Euler(0, rotation.eulerAngles.y, 0);
  }
}
