using Exiled.API.Interfaces;
using System.ComponentModel;

namespace SilentPlace
{
	public class Config : IConfig
	{
		public bool IsEnabled { get; set; } = true;

		public string WebhookUrl { get; set; } = string.Empty;
		public string WebhookProfilePicture { get; set; } = "https://pbs.twimg.com/profile_images/2818101062/9d5d27a9cd07c3a336818a203ef43e6f_400x400.jpeg";
		public string WebhookUserName { get; set; } = "Server";

		[Description("The hex colors to set each zone to.")]
		public string LczColor { get; set; } = "#FFFFFF";
		public string HczColor { get; set; } = "#FFFFFF";
		public string EntranceColor { get; set; } = "#FFFFFF";
		public string SurfaceColor { get; set; } = "#FFFFFF";
	}
}
