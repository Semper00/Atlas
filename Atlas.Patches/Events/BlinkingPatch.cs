using Atlas.Events;
using Atlas.EventSystem;
using Atlas.Entities;

using HarmonyLib;
using NorthwoodLib;

using PlayableScps;
using Mirror;

using UnityEngine;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(Scp173), nameof(Scp173.ServerHandleBlinkMessage))]
    public class BlinkingPatch
    {
        public static bool Prefix(Scp173 __instance, Vector3 blinkPos)
        {
            try
            {
				if (!__instance.BlinkReady)
					return false;

                Blinking ev = EventManager.Invoke(new Blinking(PlayersList.Get(__instance.Hub), __instance, true));

                return ev.IsAllowed;
            }
            catch (Exception e)
            {
                Manager.Exc<BlinkingPatch>(e);

                return true;
            }
        }
    }
}
