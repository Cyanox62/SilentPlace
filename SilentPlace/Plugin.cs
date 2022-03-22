using Assets._Scripts.Dissonance;
using Exiled.API.Features;
using HarmonyLib;

namespace SilentPlace
{
    public class Plugin : Plugin<Config>
    {
		internal static Plugin singleton;

		private EventHandlers ev;

		public override void OnEnabled()
		{
			base.OnEnabled();

			singleton = this;

			ev = new EventHandlers();
			Exiled.Events.Handlers.Server.RoundStarted += ev.OnRoundStart;

			Exiled.Events.Handlers.Player.Verified += ev.OnPlayerJoin;

			Exiled.Events.Handlers.Warhead.Starting += ev.OnNukeStart;
			Exiled.Events.Handlers.Warhead.Stopping += ev.OnNukeStop;
			Exiled.Events.Handlers.Warhead.Detonated += ev.OnNukeDetonate;
		}

		public override void OnDisabled()
		{
			base.OnDisabled();

			Exiled.Events.Handlers.Server.RoundStarted -= ev.OnRoundStart;

			Exiled.Events.Handlers.Player.Verified -= ev.OnPlayerJoin;

			Exiled.Events.Handlers.Warhead.Starting -= ev.OnNukeStart;
			Exiled.Events.Handlers.Warhead.Stopping -= ev.OnNukeStop;
			Exiled.Events.Handlers.Warhead.Detonated -= ev.OnNukeDetonate;

			ev = null;
		}
	}
}
