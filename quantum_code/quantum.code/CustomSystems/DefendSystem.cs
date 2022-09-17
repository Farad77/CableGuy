namespace Quantum
{
    public unsafe class DefendSystem : SystemMainThread
    {
        struct PlayerFilter
        {
            public EntityRef EntityRef;
            public PlayerID* PlayerId;
            public Shield* Shield;
            public Weapon* Weapon;
        }
        public override void Update(Frame f)
        {
            PlayerDefend(f);
            // TODO: Add AiDefend here
        }

        private static void PlayerDefend(Frame f)
        {
            var shields = f.Unsafe.FilterStruct<PlayerFilter>();
            
            var player = default(PlayerFilter);

            while (shields.Next(&player))
            {
                var input = f.GetPlayerInput(player.PlayerId->PlayerRef);
                player.Shield->IsBlocking = input->Defend.IsDown;

                if (input->Defend.WasPressed && !player.Weapon->IsEquipped)
                {
                    player.Weapon->IsEquipped = true;
                    f.Events.PlayerWeaponEquip(player.PlayerId->PlayerRef, true); 
                }
            }   
        }
    }
}