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

[CreateAssetMenu(menuName = "Quantum/AIFunctionEntityRef/DefaultAIFunctionEntityRef", order = Quantum.EditorDefines.AssetMenuPriorityStart + 3)]
public partial class DefaultAIFunctionEntityRefAsset : AIFunctionEntityRefAsset {
  public Quantum.DefaultAIFunctionEntityRef Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.DefaultAIFunctionEntityRef();
    }
    base.Reset();
  }
}

public static partial class DefaultAIFunctionEntityRefAssetExts {
  public static DefaultAIFunctionEntityRefAsset GetUnityAsset(this DefaultAIFunctionEntityRef data) {
    return data == null ? null : UnityDB.FindAsset<DefaultAIFunctionEntityRefAsset>(data);
  }
}
