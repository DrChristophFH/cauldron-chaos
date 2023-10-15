using UnityEngine;

public static class AudioHelper {
  public static void PlayClipAtPointWithSettings(AudioClip clip, Vector3 position, float volume = 1.0f, float spatialBlend = 1.0f, float minDistance = 1.0f, float maxDistance = 500.0f) {
    AudioSource audioSource = AudioPoolManager.Instance.GetPooledAudioSource();

    audioSource.clip = clip;
    audioSource.volume = volume;
    audioSource.spatialBlend = spatialBlend;
    audioSource.minDistance = minDistance;
    audioSource.maxDistance = maxDistance;
    audioSource.transform.position = position;

    audioSource.Play();
  }
}
