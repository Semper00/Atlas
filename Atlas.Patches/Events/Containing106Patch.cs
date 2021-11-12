using Atlas.Events;
using Atlas.EventSystem;
using Atlas.Entities;

using HarmonyLib;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(PlayerInteract), nameof(PlayerInteract.UserCode_CmdContain106))]
    public class Containing106Patch
    {
        public const float ContainDamage = 10000f;

        public static bool Prefix(PlayerInteract __instance)
        {
            try
            {
				if (!__instance.CanInteract)
					return false;

				if (!Map.LureSubjectContainer.allowContain 
					|| (__instance._ccm.CurRole.team == Team.SCP && __instance._ccm.CurClass != RoleType.Scp106) 
					|| !__instance.ChckDis(Map.FemurBreaker.transform.position) || Map.Scp106Contained || __instance._ccm.CurRole.team == Team.RIP)
					return false;

				foreach (Player player in PlayersList.List)
                {
					if (player.Role == RoleType.Scp106 && !player.IsGodModeEnabled)
                    {
                        Containing106 ev = EventManager.Invoke(new Containing106(player, PlayersList.Get(__instance._hub), true));

                        if (!ev.IsAllowed)
                            return false;

						player.Hub.scp106PlayerScript.Contain(__instance._hub);

						__instance.RpcContain106(__instance.gameObject);

                        Map.Scp106Contained = true;
                    }
                }

				__instance.OnInteract();

				return false;
            }
            catch (Exception e)
            {
                Manager.Exc<Containing106>(e);

                return true;
            }
        }
    }
}