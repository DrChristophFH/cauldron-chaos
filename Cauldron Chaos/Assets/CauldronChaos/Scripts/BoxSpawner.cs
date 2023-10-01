using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BoxSpawner : MonoBehaviour {
  [SerializeField]
  private int height = 5;
  [SerializeField]
  private int width = 5;

  [SerializeField]
  private GameObject boxPrefab;

  void Start() {
    // instantiate box wall
    for (int i = 0; i < height; i++) {
      for (int j = 0; j < width; j++) {
        Vector3 position = transform.position + new Vector3(i*1.2f, j*1.2f, 0);
        GameObject box = Instantiate(boxPrefab, position, Quaternion.identity);
      }
    }
  }
}
