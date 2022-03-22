using Exiled.API.Features;
using Exiled.Events.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace SilentPlace
{
	class EventHandlers
	{
		private Color lcz;
		private Color hcz;
		private Color entrance;
		private Color surface;

		private void SendWebhook(string message)
		{
			try
			{
				using (dWebHook dcWeb = new dWebHook())
				{
					dcWeb.ProfilePicture = Plugin.singleton.Config.WebhookProfilePicture;
					dcWeb.UserName = Plugin.singleton.Config.WebhookUserName;
					dcWeb.WebHook = Plugin.singleton.Config.WebhookUrl;
					dcWeb.SendMessage(message);
				}
			}
			catch (Exception x)
			{
				Log.Error("Error sending webhook: " + x.Message);
			}
		}

		private void SetRoomColors()
		{
			foreach (Room room in Map.Rooms)
			{
				switch (room.Zone)
				{
					case Exiled.API.Enums.ZoneType.LightContainment:
						room.Color = lcz;
						break;
					case Exiled.API.Enums.ZoneType.HeavyContainment:
						room.Color = hcz;
						break;
					case Exiled.API.Enums.ZoneType.Entrance:
						room.Color = entrance;
						break;
				}
			}
		}

		public static Color hexToColor(string hex)
		{
			hex = hex.Replace("0x", "");
			hex = hex.Replace("#", "");
			byte a = 255;
			byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
			byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
			byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
			if (hex.Length == 8)
			{
				a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
			}
			return new Color32(r, g, b, a);
		}

		internal void OnRoundStart()
		{
			lcz = hexToColor(Plugin.singleton.Config.LczColor);
			hcz = hexToColor(Plugin.singleton.Config.HczColor);
			entrance = hexToColor(Plugin.singleton.Config.EntranceColor);
			surface = hexToColor(Plugin.singleton.Config.SurfaceColor);

			StringBuilder sb = new StringBuilder();
			sb.Append("**Username Translations:**\n");
			int count = 1;
			foreach (Player player in Player.List)
			{
				player.ReferenceHub.dissonanceUserSetup.AdministrativelyMuted = true;
				string name = count.ToString();
				player.DisplayNickname = name;
				sb.Append($"> {count} - {player.Nickname} ({player.UserId})\n");
				count++;
			}

			SendWebhook(sb.ToString());

			SetRoomColors();
		}

		internal void OnRoundEnd(RoundEndedEventArgs ev)
		{
			foreach (Player player in Player.List)
			{
				player.ReferenceHub.dissonanceUserSetup.AdministrativelyMuted = false;
			}
		}

		internal void OnPlayerJoin(VerifiedEventArgs ev)
		{
			ev.Player.ReferenceHub.dissonanceUserSetup.AdministrativelyMuted = true;
		}

		internal void OnNukeStart(StartingEventArgs ev)
		{
			foreach (Room room in Map.Rooms) room.Color = Color.red;
		}

		internal void OnNukeStop(StoppingEventArgs ev)
		{
			SetRoomColors();
		}

		internal void OnNukeDetonate()
		{
			foreach (Room room in Map.Rooms.Where(x => x.Zone == Exiled.API.Enums.ZoneType.Surface))
			{
				room.Color = surface;
			}
		}
	}
}
