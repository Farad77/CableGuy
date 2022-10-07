using UnityEngine;
using Quantum;
using Spine.Unity;

public unsafe class PlayerSetup : MonoBehaviour
{
    //[SerializeField] private PlayerAnimation _playerAnimation = null; //THB
    [SerializeField] private BulbLightAnimation _playerAnimation = null; //THB

    private PlayerRef _playerRef;
    private EntityRef  entityRef;
    // Called from OnEntityInstantiate
    // Assigned via the inspector!
    public void Initialize()
    {
         entityRef = GetComponent<EntityView>().EntityRef;
        var playerId = QuantumRunner.Default.Game.Frames.Verified.Unsafe.GetPointer<PlayerID>(entityRef);
        _playerRef = playerId->PlayerRef;
        
        InitAnimation(entityRef);
        
        if (!QuantumRunner.Default.Game.PlayerIsLocal(_playerRef)) return;

        InitLocalCamera();
        FindObjectOfType<UIPlayerInventoryEvents>().Initialize(entityRef);
        FindObjectOfType<LocalInputCustom>().transform.parent = gameObject.transform;
        FindObjectOfType<LocalInputCustom>().transform.position = Vector3.zero;
        FindObjectOfType<UISpawnEnemy>().Initialize(_playerRef);
       
        QuantumEvent.Subscribe<EventPlayerBeginCharge>(this, EventPlayerBeginCharge);
        QuantumEvent.Subscribe<EventPlayerEndCharge>(this, EventPlayerEndCharge);

    }
    Spine.Attachment ori;
    private void EventPlayerBeginCharge(EventPlayerBeginCharge e)
    {
        if (e.EntityRef != entityRef) return;
        ori = gameObject.GetComponentInChildren<SkeletonMecanim>().skeleton.FindSlot("Arm_R").Attachment;
        // gameObject.GetComponentInChildren<CableProceduralSimple>().endPointTransform = GameObject.Find("Chain Simple End").transform;
        ProducteurEnergieUnity[] gos = GameObject.FindObjectsOfType<ProducteurEnergieUnity>();
        float min = 999;
        GameObject minGo = null;
        foreach(ProducteurEnergieUnity prod in gos)
        {
            if (Vector3.Distance(transform.position, prod.gameObject.transform.position) < min)
            {
                if (prod.gameObject == gameObject) continue;
                min = Vector3.Distance(transform.position, prod.gameObject.transform.position);
                minGo = prod.gameObject;
                
            }
        }
        gameObject.GetComponentInChildren<CableProceduralSimple>().activateAll();
        gameObject.GetComponentInChildren<CableProceduralSimple>().endPointTransform =minGo.transform;
      
        gameObject.GetComponentInChildren<SkeletonMecanim>().skeleton.FindSlot("Arm_R").Attachment = null;

    }

    private void EventPlayerEndCharge(EventPlayerEndCharge e)
    {
        if (e.EntityRef != entityRef) return;
        gameObject.GetComponentInChildren<CableProceduralSimple>().endPointTransform = null;
        
      //  Spine.Attachment ori = gameObject.GetComponentInChildren<SkeletonMecanim>().skeleton.FindSlot("Arm_R").Attachment;
        gameObject.GetComponentInChildren<SkeletonMecanim>().skeleton.FindSlot("Arm_R").Attachment = ori;
        gameObject.GetComponentInChildren<CableProceduralSimple>().deleteAll();
    }
    private void InitAnimation(EntityRef entityRef)
    {
        _playerAnimation.Initialize(_playerRef, entityRef);
    }

    private void InitLocalCamera()
    {
        var localPlayerCamera = FindObjectOfType<FollowCamera>();
        localPlayerCamera.Initialize(transform);
        localPlayerCamera.PositionCam();
    }

    private void Update()
    {
        var entityRef = GetComponent<EntityView>().EntityRef;
        var playerId = QuantumRunner.Default.Game.Frames.Verified.Unsafe.GetPointer<PlayerID>(entityRef);
        _playerRef = playerId->PlayerRef;
        if (!QuantumRunner.Default.Game.PlayerIsLocal(_playerRef)) return;
       
    }
}
