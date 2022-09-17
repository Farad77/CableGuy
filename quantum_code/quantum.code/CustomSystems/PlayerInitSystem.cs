using Photon.Deterministic;

namespace Quantum
{
    public unsafe class PlayerInitSystem : SystemSignalsOnly, ISignalOnPlayerDataSet
    {
        private const string PLAYER_PROTOTYPE = "Resources/DB/EntityPrototypes/Lion QPrefab|EntityPrototype";
        private const string DEFAULT_WEAPON_SPEC = "Resources/DB/QAssets/DefaultWeapon";
        private const string DEFAULT_ATTACK_CLIP = "Resources/DB/QAssets/DefaultAttackClip";

        public void OnPlayerDataSet(Frame f, PlayerRef playerRef)
        {
            if (DoesPlayerExist(f, playerRef)) return;
            
            var playerPrototype = f.FindAsset<EntityPrototype>(PLAYER_PROTOTYPE);
            var playerEntity = f.Create(playerPrototype);
            
            var transform = f.Unsafe.GetPointer<Transform3D>(playerEntity);
            transform->Position = FPVector3.Up;

            var playerId = f.Unsafe.GetPointer<PlayerID>(playerEntity);
            playerId->PlayerRef = playerRef;

            var weapon = f.Unsafe.GetPointer<Weapon>(playerEntity);
            weapon->WeaponSpec = f.FindAsset<WeaponSpec>(DEFAULT_WEAPON_SPEC);

            var qAnimState = f.Unsafe.GetPointer<QAnimationState>(playerEntity);
            qAnimState->AttackAnimation = f.FindAsset<ClipData>(DEFAULT_ATTACK_CLIP);
        }

        private bool DoesPlayerExist(Frame f, PlayerRef playerRef)
        {
            foreach (var player in f.GetComponentIterator<PlayerID>())
            {
                if (player.Component.PlayerRef == playerRef) {
                    return true;
                }
            }
            return false;
        }
    }
}