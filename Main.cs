using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exiled.API.Features;
using Ranked.Core.Configs;
using static Ranked.Core.EventArgs.ServerEvents;
using static Ranked.Core.EventArgs.MapEvents;
using static Ranked.Core.EventArgs.PlayerEvents;
using static Ranked.Core.EventArgs.Scp079Events;
using Exiled.Events.EventArgs.Interfaces;

namespace Ranked
{
    public class Main : Plugin<Config>
    {
        public static Main Instance;

        public override string Name => "Ranked";
        public override string Author => "GoldenPig1205";
        public override Version Version { get; } = new(1, 0, 0);
        public override Version RequiredExiledVersion { get; } = new(1, 2, 0, 5);

        public override void OnEnabled()
        {
            base.OnEnabled();
            Instance = this;

            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;

            Exiled.Events.Handlers.Scp079.GainingExperience += OnGainingExperience;

            Exiled.Events.Handlers.Player.Verified += OnVerified;
            Exiled.Events.Handlers.Player.Left += OnLeft;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Player.Hurt += OnHurt;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Escaped += OnEscaped;
            Exiled.Events.Handlers.Player.ItemAdded += OnItemAdded;
            Exiled.Events.Handlers.Player.ActivatingGenerator += OnActivatingGenerator;
            Exiled.Events.Handlers.Player.ChangingGroup += OnChangingGroup;
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStarted;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;

            Exiled.Events.Handlers.Scp079.GainingExperience -= OnGainingExperience;

            Exiled.Events.Handlers.Player.Verified -= OnVerified;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Player.Hurt -= OnHurt;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Escaped -= OnEscaped;
            Exiled.Events.Handlers.Player.ItemAdded -= OnItemAdded;
            Exiled.Events.Handlers.Player.ActivatingGenerator -= OnActivatingGenerator;
            Exiled.Events.Handlers.Player.ChangingGroup -= OnChangingGroup;

            Instance = null;
            base.OnDisabled();
        }
    }
}
