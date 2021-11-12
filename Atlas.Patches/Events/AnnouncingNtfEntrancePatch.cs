using Atlas.Entities;
using Atlas.Events;
using Atlas.EventSystem;

using HarmonyLib;
using NorthwoodLib.Pools;

using System;
using System.Text;

using Respawning.NamingRules;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(NineTailedFoxNamingRule), nameof(NineTailedFoxNamingRule.PlayEntranceAnnouncement))]
    public class AnnouncingNtfEntrancePatch
    {
        public static bool Prefix(NineTailedFoxNamingRule __instance, string regular)
        {
            try
            {
				string cassieUnitName = __instance.GetCassieUnitName(regular);

				int num = PlayersList.GetCount(x => x.IsScp && x.Role != RoleType.Scp0492);

				AnnouncingNtfEntrance ev = EventManager.Invoke(new AnnouncingNtfEntrance(num, cassieUnitName, true));

				if (!ev.IsAllowed)
					return false;

				num = ev.ScpsLeft;
				cassieUnitName = ev.GetName();

				StringBuilder stringBuilder = StringBuilderPool.Shared.Rent();

				if (ClutterSpawner.IsHolidayActive(Holidays.Christmas))
				{
					stringBuilder.Append("XMAS_EPSILON11 ");
					stringBuilder.Append(cassieUnitName);
					stringBuilder.Append("XMAS_HASENTERED ");
					stringBuilder.Append(num);
					stringBuilder.Append(" XMAS_SCPSUBJECTS");
				}
				else
				{
					stringBuilder.Append("MTFUNIT EPSILON 11 DESIGNATED ");
					stringBuilder.Append(cassieUnitName);
					stringBuilder.Append(" HASENTERED ALLREMAINING ");

					if (num == 0)
					{
						stringBuilder.Append("NOSCPSLEFT");
					}
					else
					{
						stringBuilder.Append("AWAITINGRECONTAINMENT ");
						stringBuilder.Append(num);

						if (num == 1)
						{
							stringBuilder.Append(" SCPSUBJECT");
						}
						else
						{
							stringBuilder.Append(" SCPSUBJECTS");
						}
					}
				}
	
				__instance.ConfirmAnnouncement(ref stringBuilder);

				StringBuilderPool.Shared.Return(stringBuilder);

				return false;
			}
            catch (Exception e)
            {
                Manager.Exc<AnnouncingNtfEntrancePatch>(e);

				return true;
            }
        }
    }
}
