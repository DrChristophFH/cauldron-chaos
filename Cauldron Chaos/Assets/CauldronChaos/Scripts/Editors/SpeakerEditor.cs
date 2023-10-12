using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Speaker))]
public class SpeakerEditor : Editor
{
  private Speaker speaker;

  private void OnEnable()
  {
    speaker = (Speaker)target;
  }

  public void OnSceneGUI()
  {
    for (int i = 0; i < speaker.narrationPositions.Count; i++)
    {
      EditorGUI.BeginChangeCheck();

      Vector3 newPos = Handles.PositionHandle(speaker.narrationPositions[i].position, speaker.narrationPositions[i].rotation);

      if (EditorGUI.EndChangeCheck())
      {
        Undo.RecordObject(speaker, "Change Narration Position");
        speaker.narrationPositions[i].position = newPos;
        speaker.narrationPositions[i].rotation = Quaternion.LookRotation(newPos - speaker.transform.position);
      }

      // To display position name near the handle
      Handles.Label(speaker.narrationPositions[i].position + Vector3.up * 0.5f, speaker.narrationPositions[i].positionName);
    }
  }
}
