// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial  
// declarations in another file.
// </auto-generated>

using Quantum;
using UnityEngine;

[CreateAssetMenu(menuName = "Quantum/HFSMDecision/IsWaypointInView", order = Quantum.EditorDefines.AssetMenuPriorityStart + 190)]
public partial class IsWaypointInViewAsset : HFSMDecisionAsset {
  public Quantum.IsWaypointInView Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.IsWaypointInView();
    }
    base.Reset();
  }
}

public static partial class IsWaypointInViewAssetExts {
  public static IsWaypointInViewAsset GetUnityAsset(this IsWaypointInView data) {
    return data == null ? null : UnityDB.FindAsset<IsWaypointInViewAsset>(data);
  }
}
