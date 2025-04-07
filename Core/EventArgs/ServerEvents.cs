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

            Timing.RunCoroutine(LobbyLock());
            Timing.RunCoroutine(LobbyMusic());
            Timing.RunCoroutine(LobbyHint());

            Timing.RunCoroutine(AutoSaveUserData());
        }

        public static void OnRoundStarted()
        {
            GlobalPlayer.RemoveAllClips();
        }

        public static void OnRoundEnded(RoundEndedEventArgs ev)
        {
            Timing.CallDelayed(9f, () =>
            {
                Server.ExecuteCommand("sr");
            });
        }
    }
}
