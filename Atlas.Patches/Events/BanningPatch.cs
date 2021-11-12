using Atlas.Events;
using Atlas.EventSystem;
using Atlas.Entities;

using HarmonyLib;
using NorthwoodLib;

using Mirror;

using UnityEngine;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(BanPlayer), nameof(BanPlayer.BanUser), typeof(GameObject), typeof(long), typeof(string), typeof(string), typeof(bool))]
    public class BanningAndKickingPatch
    {
        public static bool Prefix(GameObject user, long duration, string reason, string issuer, bool isGlobalBan)
        {
            try
            {
				if (isGlobalBan && GameCore.ConfigFile.ServerConfig.GetBool("gban_ban_ip", false))
					duration = int.MaxValue;

				if (duration > long.MaxValue)
					duration = long.MaxValue;

				string text = null;
				string address = user.GetComponent<NetworkIdentity>().connectionToClient.address;

				Player banned = PlayersList.Get(user);
				Player issuerPlayer = PlayersList.Get(issuer);

				if (banned == null)
                {
					Log.Error("BanningAndKickingPatch::Prefix", $"Failed to find the banned player.");

					return true;
                }

				if (issuerPlayer == null)
				{
					Log.Error("BanningAndKickingPatch::Prefix", $"Failed to find the issuing player.");

					return true;
				}

				bool onlineMode = GameCore.ConfigFile.ServerConfig.GetBool("online_mode", false);

				if (onlineMode)
					text = banned.UserId;

				if (duration <= 0)
                {
					Kicking ev = EventManager.Invoke(new Kicking(issuerPlayer, banned, reason));

					if (!ev.IsAllowed)
						return false;

					reason = ev.Reason;
                }

				if (duration > 0L && (!ServerStatic.PermissionsHandler.IsVerified || !banned.Hub.serverRoles.BypassStaff))
				{
					int @int = GameCore.ConfigFile.ServerConfig.GetInt("ban_nickname_maxlength", 30);
					bool @bool = GameCore.ConfigFile.ServerConfig.GetBool("ban_nickname_trimunicode", true);

					string text2 = string.IsNullOrEmpty(banned.Hub.nicknameSync.MyNick) ? "(no nick)" : banned.Hub.nicknameSync.MyNick;

					if (@bool)
						text2 = StringUtils.StripUnicodeCharacters(text2, "");

					if (text2.Length > @int)
						text2 = text2.Substring(0, @int);

					Banning bEv = EventManager.Invoke(new Banning(issuerPlayer, banned, 
						TimeBehaviour.CurrentTimestamp(), TimeBehaviour.GetBanExpirationTime((uint)duration), duration, reason, true));

					if (!bEv.IsAllowed)
						return false;

					long issuanceTime = bEv.Issuance.Ticks;
					long banExpirationTime = bEv.Expiery.Ticks;

					if (text != null && !isGlobalBan)
					{
						BanHandler.IssueBan(new BanDetails
						{
							OriginalName = text2,
							Id = text,
							IssuanceTime = issuanceTime,
							Expires = banExpirationTime,
							Reason = reason,
							Issuer = issuer
						}, BanHandler.BanType.UserId);

						if (!string.IsNullOrEmpty(banned.CustomUserId))
						{
							BanHandler.IssueBan(new BanDetails
							{
								OriginalName = text2,
								Id = banned.CustomUserId,
								IssuanceTime = issuanceTime,
								Expires = banExpirationTime,
								Reason = reason,
								Issuer = issuer
							}, BanHandler.BanType.UserId);
						}
					}

					if (GameCore.ConfigFile.ServerConfig.GetBool("ip_banning", false) || isGlobalBan)
					{
						BanHandler.IssueBan(new BanDetails
						{
							OriginalName = text2,
							Id = address,
							IssuanceTime = issuanceTime,
							Expires = banExpirationTime,
							Reason = reason,
							Issuer = issuer
						}, BanHandler.BanType.IP);
					}
				}

				string str = (duration > 0L) ? "banned" : "kicked";
				string text3 = "You have been " + str + ". ";

				if (!string.IsNullOrEmpty(reason))
					text3 = text3 + "Reason: " + reason;

				foreach (Player player in PlayersList.List)
				{
					if ((text != null && player.UserId == text) || (address != null && banned.IpAddress == address))
                    {
						player.Disconnect(reason);

						if (ConfigHolder.Server.LogBansToConsole)
						{
							if (duration <= 0)
                            {
								Log.Info("BAN", $"{issuerPlayer.Nickname} ({issuerPlayer.UserId}) kicked {banned.Nickname} ({banned.UserId}) for {reason}.");
                            }
						}
                    }
				}

				return false;
            }
            catch (Exception e)
            {
                Manager.Exc<BanningAndKickingPatch>(e);

                return true;
            }
        }
    }
}
