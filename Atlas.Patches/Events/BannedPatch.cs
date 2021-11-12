using Atlas.Events;
using Atlas.EventSystem;
using Atlas.Entities;
using Atlas.ModManager;

using HarmonyLib;

using System;
using System.Linq;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(BanHandler), nameof(BanHandler.IssueBan))]
    public class BannedPatch
    {
        public static bool Prefix(BanDetails ban, BanHandler.BanType banType)
        {
			try
			{
				Banned ev = EventManager.Invoke(new Banned(PlayersList.Get(ban.Issuer), ban));

				ban = ev.Details;

				if (banType == BanHandler.BanType.IP && ban.Id.Equals("localClient", StringComparison.OrdinalIgnoreCase))
				{
					return false;
				}
				else
				{
					ban.OriginalName = ban.OriginalName.Replace(";", ":");
					ban.Issuer = ban.Issuer.Replace(";", ":");
					ban.Reason = ban.Reason.Replace(";", ":");

					Misc.ReplaceUnsafeCharacters(ref ban.OriginalName, '?');
					Misc.ReplaceUnsafeCharacters(ref ban.Issuer, '?');

					if (!BanHandler.GetBans(banType).Any(x => x.Id == ban.Id))
					{
						FileManager.AppendFile(ban.ToString(), BanHandler.GetPath(banType), true);
						FileManager.RemoveEmptyLines(BanHandler.GetPath(banType));
					}
					else
					{
						BanHandler.RemoveBan(ban.Id, banType);
						BanHandler.IssueBan(ban, banType);
					}

					if (ConfigHolder.Server.LogBansToConsole)
                    {
						if (ev.Issuer != null)
							Log.Info("BAN", $"{ev.Issuer.Nickname} ({ev.Issuer.UserId}) banned {ev.Details.OriginalName} ({ev.Details.Id})" +
								$" for {ev.Details.Reason}, ban expires at {new DateTime(ev.Details.Expires).ToString("G")}");
						else
							Log.Info("BAN", $"{ev.Details.OriginalName} ({ev.Details.Id}) got banned for {ev.Details.Reason} by {ev.Details.Issuer}," +
								$"ban expires at {new DateTime(ev.Details.Expires).ToString("G")}");
                    }
				}

				return false;
			}
			catch (Exception e)
			{
				Manager.Exc<BannedPatch>(e);

				return true;
			}
        }
    }
}
