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
using Exiled.API.Enums;

namespace Ranked.Core.Variables
{
    public static class Base
    {
        public static AudioPlayer GlobalPlayer;

        public static List<Rank> Ranks = new List<Rank>
        {
            new Rank { Name = "Neutralized", Icon = "🔎", Color = "nickel", RequiredScore = 0 },
            new Rank { Name = "Safe", Icon = "🔓", Color = "light_green", RequiredScore = 173 },
            new Rank { Name = "Euclid", Icon = "🔒", Color = "yellow", RequiredScore = 330 },
            new Rank { Name = "Keter", Icon = "🔐", Color = "red", RequiredScore = 500 },
            new Rank { Name = "Thaumiel", Icon = "🔯", Color = "cyan", RequiredScore = 939 },
            new Rank { Name = "Apollyon", Icon = "🔱", Color = "deep_pink", RequiredScore = 1576 },
            new Rank { Name = "Archon", Icon = "🔰", Color = "carmine", RequiredScore = 2536 },
        };
        public static List<ushort> ScpItems = new List<ushort> { };
        public static List<ushort> SpecialWeapons = new List<ushort> { };
        public static List<Generator> Generators = new List<Generator> { };
        public static List<Player> RespawnPool = new List<Player> { };

        public static Dictionary<Player, AudioPlayer> AudioPlayers = new Dictionary<Player, AudioPlayer> { };
        public static Dictionary<string, List<string>> Audios = new Dictionary<string, List<string>>
        {
            { "BGMs", new List<string> { } },
            { "SEs", new List<string> { } },
            { "Audios", new List<string> { } }
        };
        public static Dictionary<LeadingTeam, List<Player>> Teams = new Dictionary<LeadingTeam, List<Player>>
        {
            { LeadingTeam.Anomalies, new List<Player> { } },
            { LeadingTeam.FacilityForces, new List<Player> { } },
            { LeadingTeam.ChaosInsurgency, new List<Player> { } },
        };
        public static Dictionary<Player, List<(string, float)>> PlayerScores = new Dictionary<Player, List<(string, float)>> { };
    }
}
