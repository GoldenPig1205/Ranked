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

namespace Ranked.Core.EventArgs
{
    public static class PlayerEvents
    {
        public static void OnVerified(VerifiedEventArgs ev)
        {
        }
    }
}
