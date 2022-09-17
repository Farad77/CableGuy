using UnityEngine;
using Quantum;

public unsafe class PlayerSetup : MonoBehaviour
{
    [SerializeField] private PlayerAnimation _playerAnimation = null;
    
    private PlayerRef _playerRef;

    // Called from OnEntityInstantiate
    // Assigned via the inspector!
    public void Initialize()
    {
        var entityRef = GetComponent<EntityView>().EntityRef;
        var playerId = QuantumRunner.Default.Game.Frames.Verified.Unsafe.GetPointer<PlayerID>(entityRef);
        _playerRef = playerId->PlayerRef;
        
        InitAnimation(entityRef);
        
        if (!QuantumRunner.Default.Game.PlayerIsLocal(_playerRef)) return;

        InitLocalCamera();
        FindObjectOfType<UIPlayerInventoryEvents>().Initialize(entityRef);
        FindObjectOfType<UISpawnEnemy>().Initialize(_playerRef);
    }

    private void InitAnimation(EntityRef entityRef)
    {
        _playerAnimation.Initialize(_playerRef, entityRef);
    }

    private void InitLocalCamera()
    {
        var localPlayerCamera = FindObjectOfType<FollowCamera>();
        localPlayerCamera.Initialize(transform);
    }
}
