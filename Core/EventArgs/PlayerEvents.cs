using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MapEditorReborn.API.Features.Objects;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Ranked.Core.Variables.Base;
using static Ranked.Core.Functions.Base;
using static Ranked.Core.Classes.FileManager;
using System.Diagnostics.Eventing.Reader;
using InventorySystem.Items;
using MultiBroadcast.API;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Toys;
using InventorySystem.Items.Firearms.Attachments;
using MapEditorReborn.API.Enums;
using MapEditorReborn.API;
using Exiled.API.Features.Items;
using RemoteAdmin;
using static System.Net.Mime.MediaTypeNames;
using Ranked.Core.Classes;
using Exiled.API.Features.Roles;
using Respawning;
using PlayerRoles.FirstPersonControl;
using Exiled.Events.Commands.Hub;
using RelativePositioning;
using Exiled.API.Extensions;
using Mirror;
using Microsoft.Win32;

namespace Ranked.Core.EventArgs
{
    public static class PlayerEvents
    {
        public static void OnVerified(VerifiedEventArgs ev)
        {
            // Initialize player in the database
            int rankPoints = GetRankPoints(ev.Player.UserId);

            Rank rank = Ranks
                .Where(r => rankPoints >= r.RequiredScore)
                .OrderByDescending(r => r.RequiredScore)
                .FirstOrDefault();

            ev.Player.RankName = $"{rank.Icon} {rank.Name}";
            ev.Player.RankColor = rank.Color;

            UpdateRankLevel(ev.Player.UserId, rank.Name);
        }

        public static void OnLeft(LeftEventArgs ev)
        {
            // Deduct RP for leaving mid-round
            if (!Round.IsLobby && !Round.IsEnded)
            {
                AddRankPoints(ev.Player.UserId, -10, "PlayerLeft");
            }
        }

        public static void OnHurt(HurtEventArgs ev)
        {
            if (ev.Attacker != null && !ev.Attacker.IsScp)
            {
                if (HitboxIdentity.IsEnemy(ev.Attacker.ReferenceHub, ev.Player.ReferenceHub))
                {
                    AddRankPoints(ev.Attacker.UserId, (int)(ev.DamageHandler.Damage / 100), "DamageDealt");
                }
                else
                {
                    AddRankPoints(ev.Attacker.UserId, -(int)(ev.DamageHandler.Damage / 50), "FriendlyFire");
                }
            }
        }

        public static IEnumerator<float> OnDied(DiedEventArgs ev)
        {
            if (ev.Attacker != null && ev.Attacker.IsScp)
            {
                if (HitboxIdentity.IsEnemy(ev.Attacker.ReferenceHub, ev.Player.ReferenceHub))
                {
                    AddRankPoints(ev.Attacker.UserId, 1, "Kill");
                }
            }
            yield break;
        }

        public static void OnEscaped(EscapedEventArgs ev)
        {
            if (new List<RoleTypeId> { RoleTypeId.ClassD, RoleTypeId.Scientist }.Contains(ev.Player.Role.Type))
            {
                if (ev.Player.IsCuffed)
                {
                    AddRankPoints(ev.Player.UserId, 1, "CuffedEscape");
                    AddRankPoints(ev.Player.Cuffer.UserId, 2, "CaptorBonus");
                }
                else
                {
                    AddRankPoints(ev.Player.UserId, 3, "PureEscape");
                }
            }
        }
    }
}
