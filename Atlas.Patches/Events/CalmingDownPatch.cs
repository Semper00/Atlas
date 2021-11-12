using Atlas.Entities;
using Atlas.Enums;
using Atlas.Events;
using Atlas.EventSystem;
using PlayableScps;

using HarmonyLib;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.EndEnrage))]
    public class CalmingDownPatch
    {
        public static bool Prefix(Scp096 __instance)
        {
            try
            {
                CalmingDown ev = EventManager.Invoke(new CalmingDown(PlayersList.Get(__instance.Hub), __instance, true));

                if (!ev.IsAllowed)
                    return false;

                __instance.EndCharge();
                __instance.SetMovementSpeed(0f);
                __instance.SetJumpHeight(4f);
                __instance.ResetShield();

                __instance.PlayerState = Scp096PlayerState.Calming;
                __instance.AddedTimeThisRage = 0f;
                __instance._calmingTime = 6f;
                __instance.EnrageTimeLeft = 0f;

                __instance._targets.Clear();

                __instance._chargeCooldownPenaltyAmount = 0;

                __instance.Hub.characterClassManager.netIdentity.
                    connectionToClient.Send(new PlayableScps.Messages.Scp096ToSelfMessage(__instance.EnrageTimeLeft, __instance._chargeCooldown), 0);

                return false;
            }
            catch (Exception e)
            {
                Manager.Exc<CalmingDownPatch>(e);

                return true;
            }
        }
    }
}
