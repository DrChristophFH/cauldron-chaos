using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class AudioPoolManager : MonoBehaviour {
  public static AudioPoolManager Instance;

  [SerializeField]
  private List<AudioSource> pooledSources = new List<AudioSource>();
  [SerializeField]
  private int poolSize = 10;

  private void Awake() {
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
      return;
    }

    for (int i = 0; i < poolSize; i++) {
      AddNewSourceToPool().gameObject.SetActive(false);
    }
  }

  private AudioSource AddNewSourceToPool() {
    GameObject src = new GameObject("PooledAudio");
    src.AddComponent<AutoDeactivate>(); // implicitly adds AudioSource
    AudioSource audioSrc = src.GetComponent<AudioSource>();
    audioSrc.playOnAwake = false;
    src.transform.SetParent(transform);
    pooledSources.Add(audioSrc);
    return audioSrc;
  }

  public AudioSource GetPooledAudioSource() {
    foreach (var src in pooledSources) {
      if (!src.gameObject.activeInHierarchy) {
        src.gameObject.SetActive(true);
        return src;
      }
    }

    // If none are available, create a new one and add it to the pool.
    return AddNewSourceToPool();
  }
}
