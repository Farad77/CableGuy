using UnityEngine;
using Quantum;

public unsafe class ElectricSheepSetup : MonoBehaviour
{
    [SerializeField] private ElectricSheepAnimation _electricSheepAnimation = null; //THB
    // Called from OnEntityInstantiate
    // Assigned via the inspector!
    private EntityRef me;
    private Quaternion ori;
    public void Initialize()
    {
        var entityRef = GetComponent<EntityView>().EntityRef;
        me = entityRef;
        var electricSheepID = QuantumRunner.Default.Game.Frames.Verified.Unsafe.GetPointer<ElectricSheepID>(entityRef);
      
        _electricSheepAnimation.Initialize(entityRef);
      //  ori=this.gameObject.transform.rotation;
    }

    public void Update()
    {
      //  this.gameObject.transform.rotation = ori;
    }
}
