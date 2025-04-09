using Exiled.API.Extensions;
using Exiled.API.Features;
using MEC;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Ranked.Core.Variables.Base;
using static Ranked.Core.Functions.Base;
using InventorySystem.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Items;
using PluginAPI.Events;
using PlayerRoles.FirstPersonControl;
using PlayerRoles;
using System.Threading;
using Ranked.Core.Classes;
using System;
using Exiled.API.Enums;
using Respawning;
using Respawning.Waves;

namespace Ranked.Core.IEnumerators
{
    public static class Base
    {
        public static IEnumerator<float> LobbyLock()
        {
            while (Round.IsLobby)
            {
                if (Server.PlayerCount >= 10)
                {
                    Round.IsLobbyLocked = false;
                }
                else
                {
                    Round.IsLobbyLocked = true;
                }

                yield return Timing.WaitForSeconds(1);
            }
        }

        public static IEnumerator<float> LobbyMusic()
        {
            AudioClipPlayback music = null;

            while (Round.IsLobby)
            {
                if (Server.PlayerCount >= 20)
                    music = GlobalPlayer.AddClip("Clarx - Bones   Future House   NCS - Copyright Free Music", 0.3f);

                else if (Server.PlayerCount >= 15)
                    music = GlobalPlayer.AddClip("Escape the Backrooms OST - Escapee", 0.3f);

                else if (Server.PlayerCount >= 10)
                    music = GlobalPlayer.AddClip("Escape the Backrooms OST - Menu", 0.2f);

                else if (Server.PlayerCount >= 5)
                    music = GlobalPlayer.AddClip("Escape the Backrooms - Level 1 Ambience", 0.2f);

                else
                    music = GlobalPlayer.AddClip("Escape the Backrooms - Level 188 Courtyard Ambience", 0.2f);

                yield return Timing.WaitForSeconds((float)music.Duration.TotalSeconds);
            }
        }

        public static IEnumerator<float> LobbyHint()
        {
            while (Round.IsLobby)
            {
                List<string> queue = new List<string>();

                string c(int num)
                {
                    switch (num)
                    {
                        case 1:
                            return $"<color=#ffd700>#{num}</color>";

                        case 2:
                            return $"<color=#c0c0c0>#{num}</color>";

                        case 3:
                            return $"<color=#cd7f32>#{num}</color>";

                        default:
                            return $"#{num}";
                    }
                }

                foreach (var user in UsersManager.UsersCache.OrderByDescending(x =>
                {
                    int exp;
                    return int.TryParse(x.Value[1], out exp) ? exp : 0;
                }).Take(10))
                {
                    try
                    {
                        string Name = user.Value[0];
                        string RP = user.Value[1];
                        float rp = float.Parse(RP);
                        Rank rank = Ranks.Where(rank => rp >= rank.RequiredScore).OrderByDescending(rank => rank.RequiredScore).FirstOrDefault();

                        queue.Add($"{c(queue.Count() + 1)} - <b>{Name}</b>(<color=orange>{RP}</color>)\n<size=15>[<color={ChangeColor(rank.Color)}>{rank.Icon} {rank.Name}</color>] <color=#D9D9D9>{user.Key}</color></size>");
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }

                foreach (var player in Player.List.Where(x => !x.IsNPC))
                {
                    List<string> user = UsersManager.UsersCache[player.UserId];
                    float rp = float.Parse(user[1]);
                    Rank rank = Ranks.Where(rank => rp >= rank.RequiredScore).OrderByDescending(rank => rank.RequiredScore).FirstOrDefault();

                    float nextRankScore = Ranks.Where(r => r.RequiredScore > rp).OrderBy(r => r.RequiredScore).FirstOrDefault()?.RequiredScore ?? 0;
                    float rpToNextRank = nextRankScore > 0 ? nextRankScore - rp : 0;

                    player.ShowHint(
$"""
<align=left>
[<color={ChangeColor(rank.Color)}>{rank.Icon} {rank.Name}</color>] - (<color=orange>{user[1]}</color>)
<size=20>다음 랭크까지 <color=orange>{rpToNextRank}</color>RP 남음</size>

<size=25><b><color=#00FF55>D</color><color=#11FB66>A</color><color=#22F878>O</color><color=#33F489>N</color><color=#44F19B>:</color> <color=#66EABE>R</color><color=#60EDC8>a</color><color=#5BF1D2>n</color><color=#56F4DD>k</color><color=#51F8E7>e</color><color=#4CFBF1>d</color> <color=red>#경쟁전</color> 리더보드</b></size>

<size=25>{string.Join("\n", queue)}</size>
</align>




""", 1.2f);
                }

                yield return Timing.WaitForSeconds(1);
            }
        }

        public static IEnumerator<float> AutoSaveUserData()
        {
            while (true)
            {
                UsersManager.LoadUsers();

                yield return Timing.WaitForSeconds(1f);
            }
        }
    }
}
