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
            List<string> DefaultValues = Enumerable.Repeat("0", 15).ToList();

            if (!UsersManager.UsersCache.ContainsKey(ev.Player.UserId))
            {
                UsersManager.AddUser(ev.Player.UserId, DefaultValues);

                UsersManager.SaveUsers();
            }
            else
            {
                UsersManager.UsersCache[ev.Player.UserId][0] = ev.Player.Nickname;
                UsersManager.SaveUsers();
            }

            if (Round.IsLobby) 
            {
                float rp = float.Parse(UsersManager.UsersCache[ev.Player.UserId][1]);
                Rank rank = Ranks.Where(rank => rp >= rank.RequiredScore).OrderByDescending(rank => rank.RequiredScore).FirstOrDefault();

                ev.Player.RankName = $"{rank.Icon} {rank.Name}";
                ev.Player.RankColor = rank.Color;

                ev.Player.Role.Set(RoleTypeId.Spectator);
            }
            else
            {
                ev.Player.DisplayNickname = $"Player - {Random.Range(0, 10000).ToString("D4")}";
                ev.Player.RankName = null;
                ev.Player.RankColor = null;

                ev.Player.ShowHint($"경쟁전에 중도 참여하였습니다.");
            }
        }

        public static void OnLeft(LeftEventArgs ev)
        {
            if (PlayerScores.ContainsKey(ev.Player))
                PlayerScores.Remove(ev.Player);

            if (!Round.IsLobby && !Round.IsEnded)
            {
                ev.Player.AddRP(-10);
            }
        }

        public static void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Reason == SpawnReason.RoundStart)
            {
                ev.Player.DisplayNickname = $"Player - {Random.Range(0, 10000).ToString("D4")}";
                ev.Player.RankName = null;
                ev.Player.RankColor = null;
            }

            if (ev.Player.IsAlive)
            {
                if (!PlayerScores.ContainsKey(ev.Player))
                    PlayerScores.Add(ev.Player, new List<(string, float)>());

                foreach (var team in Teams)
                {
                    if (team.Value.Contains(ev.Player))
                    {
                        team.Value.Remove(ev.Player);
                    }
                }

                if (Teams.ContainsKey(ev.Player.LeadingTeam))
                    Teams[ev.Player.LeadingTeam].Add(ev.Player);
            }
        }

        public static void OnHurt(HurtEventArgs ev)
        {
            if (Round.IsEnded)
                return;
                
            if (ev.Attacker != null && !ev.Attacker.IsScp)
            {
                if (HitboxIdentity.IsEnemy(ev.Attacker.ReferenceHub, ev.Player.ReferenceHub))
                {
                    ev.Attacker.AddScore("처치 기여", ev.DamageHandler.Damage / 100);
                }
                else
                {
                    ev.Attacker.AddScore("아군 사격", -(ev.DamageHandler.Damage / 50));
                }
            }
        }

        public static IEnumerator<float> OnDied(DiedEventArgs ev)
        {
            if (Round.IsEnded)
                yield break;

            if (ev.Attacker != null && ev.Attacker.IsScp)
            {
                if (HitboxIdentity.IsEnemy(ev.Attacker.ReferenceHub, ev.Player.ReferenceHub))
                {
                    ev.Attacker.AddScore("인간 사살", 1);
                }
            }
        }

        public static void OnEscaped(EscapedEventArgs ev)
        {
            if (Round.IsEnded)
                return;

            if (new List<RoleTypeId> 
            { 
                RoleTypeId.ClassD, 
                RoleTypeId.Scientist
            }.Contains(ev.Player.Role.Type))
            {
                if (ev.Player.IsCuffed)
                {
                    ev.Player.AddScore("체포 탈출", 1);
                    ev.Player.Cuffer.AddScore("체포자", 2);
                }
                else
                {
                    ev.Player.AddScore("순수 탈출", 3);
                }
            }
        }

        public static void OnItemAdded(ItemAddedEventArgs ev)
        {
            if (Round.IsEnded)
                return;

            if (ev.Item.Type.ToString().Contains("SCP"))
            {
                if (!ScpItems.Contains(ev.Item.Serial))
                {
                    ScpItems.Add(ev.Item.Serial);

                    ev.Player.AddScore("SCP 아이템 확보", 1);
                }
            }

            if (new List<ItemType> 
            {
                ItemType.Jailbird,
                ItemType.MicroHID,
                ItemType.ParticleDisruptor
            }.Contains(ev.Item.Type))
            {
                SpecialWeapons.Add(ev.Item.Serial);

                ev.Player.AddScore("특수 무기 확보", 1);
            }
        }

        public static void OnActivatingGenerator(ActivatingGeneratorEventArgs ev)
        {
            if (Round.IsEnded)
                return;

            if (!Generators.Contains(ev.Generator))
            {
                Generators.Add(ev.Generator);

                ev.Player.AddScore("발전기 가동", 1);
            }
        }

        public static void OnChangingGroup(ChangingGroupEventArgs ev)
        {
            ulong permission = ev.Player.Group.Permissions;

            Timing.CallDelayed(1, () =>
            {
                ev.Player.Group.Permissions = permission;
            });

            ev.IsAllowed = false;
        }
    }
}
