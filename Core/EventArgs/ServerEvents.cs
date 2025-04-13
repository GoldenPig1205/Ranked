using Exiled.API.Features;
using InventorySystem.Configs;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Ranked.Core.Variables.Base;
using static Ranked.Core.Functions.Base;
using static Ranked.Core.IEnumerators.Base;
using static Ranked.Core.Extensions.Base;
using MultiBroadcast.API;
using Exiled.API.Extensions;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Serializable;
using Exiled.API.Enums;
using Interactables.Interobjects.DoorUtils;
using MapEditorReborn.API.Enums;
using PlayerRoles;
using Ranked.Core.Classes;
using Exiled.Events.EventArgs.Server;
using System.Net.Sockets;

namespace Ranked.Core.EventArgs
{
    public static class ServerEvents
    {
        public static IEnumerator<float> OnWaitingForPlayers()
        {
            yield return Timing.WaitForSeconds(1);

            UsersManager.LoadUsers();

            foreach (var _audioClip in System.IO.Directory.GetFiles(Paths.Plugins + "/audio/"))
            {
                string name = _audioClip.Replace(Paths.Plugins + "/audio/", "").Replace(".ogg", "");

                Audios["Audios"].Add(name);

                AudioClipStorage.LoadClip(_audioClip, name);
            }

            foreach (var _audioClip in System.IO.Directory.GetFiles(Paths.Configs + "/Backroom/BGMs/"))
            {
                string name = _audioClip.Replace(Paths.Configs + "/Backroom/BGMs/", "").Replace(".ogg", "");

                Audios["BGMs"].Add(name);

                AudioClipStorage.LoadClip(_audioClip, name);
            }

            foreach (var _audioClip in System.IO.Directory.GetFiles(Paths.Configs + "/Backroom/SEs/"))
            {
                string name = _audioClip.Replace(Paths.Configs + "/Backroom/SEs/", "").Replace(".ogg", "");

                Audios["SEs"].Add(name);

                AudioClipStorage.LoadClip(_audioClip, name);
            }

            GlobalPlayer = AudioPlayer.CreateOrGet($"Global AudioPlayer", onIntialCreation: (p) =>
            {
                Speaker speaker = p.AddSpeaker("Main", isSpatial: false, maxDistance: 5000);
            });

            Timing.RunCoroutine(LobbyMusic());
            Timing.RunCoroutine(LobbyHint());

            Timing.RunCoroutine(AutoSaveUserData());

            Timing.RunCoroutine(ShowTeam());
        }

        public static void OnRoundStarted()
        {
            GlobalPlayer.RemoveAllClips();
        }

        public static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Timing.CallDelayed(9, () =>
            {
                Server.ExecuteCommand("sr");
            });

            List<(Player, int)> mvp = new List<(Player, int)>();
            string formatter(int i)
            {
                if (i > 0)
                {
                    return $"<color=#40FF00>+{i}</color>";
                }
                else if (i < 0)
                {
                    return $"<color=red>{i}</color>";
                }
                else
                {
                    return $"{i}";
                }
            }

            foreach (var player in Player.List.Where(x => PlayerScores.ContainsKey(x)))
            {
                if (player.IsAlive)
                {
                    player.AddScore("생존", 1);
                }

                if (Teams.Values.Any(x => x.Contains(player)))
                {
                    if (Teams.ContainsKey(ev.LeadingTeam))
                    {
                        if (Teams[ev.LeadingTeam].Contains(player))
                            player.AddScore("승리", 10);

                        else
                            player.AddScore("패배", -10);
                    }
                }

                List<string> queue = new List<string>();
                float rp = 0;

                foreach (var score in PlayerScores[player])
                {
                    rp += score.Item2;

                    queue.Add($"{score.Item1} ({formatter((int)score.Item2)})");
                }

                player.ShowHint(
 $"""
        <align=left>
        <size=20>
        <i>
        {string.Join("\n", queue)}
        </i>
        </size>

        <b>총합 : {formatter((int)rp)}RP</b>
        </align>









        """, ev.TimeToRestart);
                player.AddRP((int)rp);

                mvp.Add((player, (int)rp));
            }

            int maxRp = mvp.Max(x => x.Item2);
            List<Player> mvps = mvp.Where(x => x.Item2 == maxRp).Select(x => x.Item1).ToList();

            foreach (var player in Player.List)
            {
                float rp = float.Parse(UsersManager.UsersCache[player.UserId][1]);
                Rank rank = Ranks.Where(rank => rp >= rank.RequiredScore).OrderByDescending(rank => rank.RequiredScore).FirstOrDefault();

                player.DisplayNickname = "";
                player.RankName = $"{rank.Icon} {rank.Name}";
                player.RankColor = rank.Color;

                player.AddBroadcast(20, $"<b><size=25>이번 라운드의 <color=#ffd700>MVP</color>: {string.Join(",", mvps.Select(x => x.Nickname))}({formatter(maxRp)}RP)</size></b>");
            }
        }
    }
}
