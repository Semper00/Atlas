using Atlas.Entities;
using Atlas.Events;
using Atlas.EventSystem;

using HarmonyLib;

using System;

using InventorySystem.Items.Firearms.Attachments;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(WorkstationController), nameof(WorkstationController.ServerInteract))]
    public class ActivatingWorkstationPatch
    {
        public static bool Prefix(WorkstationController __instance, ReferenceHub ply, byte colliderId)
        {
            try
            {
                Workstation station = Workstation.Get(__instance);

                if (station == null)
                    return true;

                Player player = PlayersList.Get(ply);

                if (player == null)
                    return true;

                ActivatingWorkstation ev = EventManager.Invoke(new ActivatingWorkstation(player, station, true));

                if (!ev.IsAllowed)
                    return false;

                if (colliderId == __instance._activateCollder.ColliderId && __instance.Status == 0)
                {
                    __instance.NetworkStatus = 1;

                    __instance._serverStopwatch.Restart();
                }

                return false;
            }
            catch (Exception e)
            {
                Manager.Exc<ActivatingWorkstationPatch>(e);

                return true;
            }
        }
    }
}
