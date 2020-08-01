using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Exiled.API.Extensions;
using Exiled.API.Interfaces;
using Exiled.Events;
using Handlers = Exiled.Events.Handlers;

namespace BreakDoorPlugin
{
    public class Plugin : Plugin<Config>
    {
		public override string Author { get; } = "HardFoxy";
		public override string Name { get; } = "door_explosion";
		public override string Prefix { get; } = "door_explosion";
		public override Version Version { get; } = Assembly.GetExecutingAssembly().GetName().Version;
		public override Version RequiredExiledVersion { get; } = new Version(2, 0, 7);

		public EventHandlers EventHandlers;
		public override void OnEnabled()
		{

			try
			{
				EventHandlers = new EventHandlers(this);
				base.OnEnabled();
				RegisterEvents();
			}
			catch (Exception e)
			{
				Log.Error($"Loading error: {e}");
			}
		}
		internal void RegisterEvents()
		{
			Handlers.Player.InteractingDoor += EventHandlers.TryBreakDoor;
			Handlers.Server.RoundStarted += EventHandlers.RoundStart;
			
		}

		public override void OnDisabled()
		{
			Handlers.Player.InteractingDoor -= EventHandlers.TryBreakDoor;
		}

		public override void OnReloaded()
		{

		}
	}
}
