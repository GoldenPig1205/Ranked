using AdminToys;
using Ranked.Core.Extensions;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Hints;
using MapEditorReborn.API.Features;
using MapEditorReborn.API.Features.Objects;
using MapEditorReborn.API.Features.Serializable;
using MEC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utf8Json.Internal.DoubleConversion;
using Utf8Json.Resolvers.Internal;
using static Ranked.Core.Variables.Base;
using Ranked.Core.Classes;

namespace Ranked.Core.Functions
{
    public static class Base
    {
        public static List<T> EnumToList<T>()
        {
            Array items = Enum.GetValues(typeof(T));
            List<T> itemList = new List<T>();

            foreach (T item in items)
            {
                if (!item.ToString().Contains("None"))
                    itemList.Add(item);
            }

            return itemList;
        }

        public static string ChangeColor(string colorName)
        {
            if (ColorUtility.TryParseHtmlString(colorName, out Color color))
            {
                return $"#{ColorUtility.ToHtmlStringRGB(color)}";
            }

            switch (colorName.ToLower())
            {
                case "nickel":
                    return "#727472";
                case "light_green":
                    return "#90EE90";
                case "cyan":
                    return "#00FFFF";
                case "deep_pink":
                    return "#FF1493";
                case "carmine":
                    return "#960018";
                default:
                    throw new ArgumentException($"Unknown color name: {colorName}");
            }
        }


        public static void AddRP(this Player player, int amount)
        {
            UsersManager.UsersCache[player.UserId][1] = Math.Max(0, int.Parse(UsersManager.UsersCache[player.UserId][1]) + amount).ToString();
            UsersManager.SaveUsers();
        }

        public static void SetRP(this Player player, int amount)
        {
            UsersManager.UsersCache[player.UserId][1] = amount.ToString();
            UsersManager.SaveUsers();
        }

        public static void AddScore(this Player player, string workName, float score)
        {
            int maxPlayers = Server.MaxPlayerCount;
            int currentPlayers = Server.PlayerCount;

            if (currentPlayers < maxPlayers)
            {
                float factor = (float)currentPlayers / maxPlayers;
                score *= factor;
            }

            (string, float) info = PlayerScores[player].FirstOrDefault(x => x.Item1 == workName);

            if (info != default((string, float)))
            {
                PlayerScores[player].Remove(info);
                info.Item2 += score;
                PlayerScores[player].Add(info);
            }
            else
            {
                PlayerScores[player].Add((workName, score));
            }
        }
    }
}
