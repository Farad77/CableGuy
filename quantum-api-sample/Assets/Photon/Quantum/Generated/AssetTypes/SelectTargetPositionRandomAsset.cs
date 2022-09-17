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

[CreateAssetMenu(menuName = "Quantum/AIAction/SelectTargetPositionRandom", order = Quantum.EditorDefines.AssetMenuPriorityStart + 18)]
public partial class SelectTargetPositionRandomAsset : AIActionAsset {
  public Quantum.SelectTargetPositionRandom Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.SelectTargetPositionRandom();
    }
    base.Reset();
  }
}

public static partial class SelectTargetPositionRandomAssetExts {
  public static SelectTargetPositionRandomAsset GetUnityAsset(this SelectTargetPositionRandom data) {
    return data == null ? null : UnityDB.FindAsset<SelectTargetPositionRandomAsset>(data);
  }
}
