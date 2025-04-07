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

        public static void AddRP(this Player player, int amount)
        {
            UsersManager.UsersCache[player.UserId][1] = (int.Parse(UsersManager.UsersCache[player.UserId][1]) + amount).ToString();
        }

        public static void RemoveRP(this Player player, int amount)
        {
            if (int.Parse(UsersManager.UsersCache[player.UserId][1]) - amount < 0)
                amount = int.Parse(UsersManager.UsersCache[player.UserId][1]);

            UsersManager.UsersCache[player.UserId][1] = (int.Parse(UsersManager.UsersCache[player.UserId][1]) - amount).ToString();
        }

        public static void SetRP(this Player player, int amount)
        {
            UsersManager.UsersCache[player.UserId][1] = amount.ToString();
        }
    }
}
