using Rocket.Core;
using Rocket.Core.Plugins;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger = Rocket.Core.Logging.Logger;

#nullable enable
namespace MrKan.DeathRewards
{
    public class Plugin : RocketPlugin<Config>
    {
        public static Plugin? Instance { get; private set; }
        private Dictionary<Player, DateTime> LastUses { get; set; } = new();

        protected override void Load()
        {
            Instance = this;

            PlayerLife.OnRevived_Global += OnPlayerRevived;

            Logger.Log($"Successfully loaded {Configuration.Instance.Groups.Count} groups");
        }

        private void OnPlayerRevived(PlayerLife life)
        {
            try
            {
                var group = GetPlayerGroup(life.player);
                if (group == null)
                {
                    return;
                }

                if (!CheckCooldown(life.player))
                {
                    return;
                }

                ResetCooldown(life.player);

                GiveGroupItems(life.player, group);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "Failed to give death reward");
            }
        }

        private bool CheckCooldown(Player player)
        {
            if (LastUses.TryGetValue(player, out var lastUse))
            {
                return (DateTime.Now - lastUse).TotalSeconds > Configuration.Instance.Cooldown;
            }
            return true;
        }

        private void ResetCooldown(Player player)
        {
            LastUses[player] = DateTime.Now;
        }

        /// <summary>
        /// Выбирает группу игрока в зависимости от его пермишнов. Чем ниже группа в конфиге, тем она приоритетнее.
        /// </summary>
        private Group? GetPlayerGroup(Player player)
        {
            var playerGroups = R.Permissions.GetGroups(UnturnedPlayer.FromPlayer(player), true).Select(g => g.Id).ToHashSet();

            // Reverse для приоритета нижних групп
            foreach (var group in Enumerable.Reverse(Configuration.Instance.Groups)) 
            {
                if (playerGroups.Contains(group.GroupId))
                {
                    return group;
                }
            }

            return null;
        }

        private void GiveGroupItems(Player player, Group group)
        {
            // SDG.Unturned.Item нужно создавать для каждого предмета, иначе будут баги
            // предварительно находить ассет нет смысла, т. к. он в любом случае ищется заново в конструкторе
            foreach (var item in group.Items) 
            { 
                for (int i = 0; i < item.Count; i++)
                {
                    player.inventory.forceAddItem(new SDG.Unturned.Item(item.Id, EItemOrigin.ADMIN), true);
                }
            }
            Logger.Log($"Given items of group {group.GroupId} to player {player.channel.owner.playerID.characterName} ({player.channel.owner.playerID.steamID})");
        }

        protected override void Unload()
        {
            Instance = null;

            PlayerLife.OnRevived_Global -= OnPlayerRevived;
        }
    }
}
