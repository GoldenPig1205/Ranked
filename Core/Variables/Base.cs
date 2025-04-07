using Exiled.API.Features;
using MapEditorReborn.API.Features.Objects;
using PlayerStatsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Ranked.Core.Classes;

namespace Ranked.Core.Variables
{
    public static class Base
    {
        public static AudioPlayer GlobalPlayer;
        public static bool IsUsersFileLoaded = false;

        public static Dictionary<Player, AudioPlayer> AudioPlayers = new Dictionary<Player, AudioPlayer> { };
        public static Dictionary<string, List<string>> Audios = new Dictionary<string, List<string>> 
        {
            { "BGMs", new List<string> { } },
            { "SEs", new List<string> { } },
            { "Audios", new List<string> { } }
        };
        public static Dictionary<Player, List<(string, float)>> PlayerScores = new Dictionary<Player, List<(string, float)>> { };
    }
}
