using UnityEngine;
using Quantum;

public unsafe class ElectricSheepSetup : MonoBehaviour
{
    [SerializeField] private ElectricSheepAnimation _electricSheepAnimation = null; //THB
    // Called from OnEntityInstantiate
    // Assigned via the inspector!
    public void Initialize()
    {
        var entityRef = GetComponent<EntityView>().EntityRef;
        var electricSheepID = QuantumRunner.Default.Game.Frames.Verified.Unsafe.GetPointer<ElectricSheepID>(entityRef);

        _electricSheepAnimation.Initialize(entityRef);
    }
}
