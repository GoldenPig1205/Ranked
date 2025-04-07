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

namespace Ranked.Core.IEnumerators
{
    public static class Base
    {
        public static IEnumerator<float> LobbyLock()
        {
            while (Round.IsLobby)
            {
                if (Server.PlayerCount == Server.MaxPlayerCount)
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
                {
                    music = GlobalPlayer.AddClip("Escape the Backrooms OST - Escapee", 0.5f);
                }
                else if (Server.PlayerCount >= 10)
                {
                    music = GlobalPlayer.AddClip("Escape the Backrooms OST - Menu", 0.3f);
                }
                else
                {
                    music = GlobalPlayer.AddClip("Escape the Backrooms - Level 188 Courtyard Ambience", 0.2f);
                }

                yield return Timing.WaitForSeconds((float)music.Duration.TotalSeconds);
            }
        }

        public static IEnumerator<float> LobbyHint()
        {
            while (Round.IsLobby)
            {
                foreach (var player in Player.List)
                {
                    player.ShowHint("\n");
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
