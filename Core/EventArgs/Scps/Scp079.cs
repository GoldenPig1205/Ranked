using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Ranked.Core.Variables.Base;
using static Ranked.Core.Functions.Base;
using Ranked.Core.Classes;
using Exiled.Events.EventArgs.Scp079;

namespace Ranked.Core.EventArgs
{
    public static class Scp079Events
    {
        public static void OnGainingExperience(GainingExperienceEventArgs ev)
        {
            ev.Player.AddScore("경험치 획득", ev.Amount / 50.0f);
        }
    }
}
