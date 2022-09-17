#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Quantum;

public partial class ClipDataAsset
{
  [SerializeField] private AnimationClip _attackClip = null;
  public void Bake()
  {
    // The quantum ClipData can be found in the "Settings" of the Unity ClipDataAsset. 
    DefineTimeEvents(ref this.Settings, _attackClip);

    this.Settings.ClipName = _attackClip.name + "QAsset";
    this.Settings.TotalLength = _attackClip.length.ToFP();

    EditorUtility.SetDirty(this);
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
  }

  private void DefineTimeEvents(ref ClipData clipData, AnimationClip unityClip)
  {
    var events = unityClip.events;

    for (int i = 0; i < events.Length; i++)
    {
      // Not so flexible, due to Unity animation events API
      if ((TimeEventType)events[i].intParameter == TimeEventType.Start)
      {
        clipData.StartEvent.Time = events[i].time.ToFP();
      }

      if ((TimeEventType)events[i].intParameter == TimeEventType.End)
      {
        clipData.EndEvent.Time = events[i].time.ToFP();
      }
    }
  }
}
#endif