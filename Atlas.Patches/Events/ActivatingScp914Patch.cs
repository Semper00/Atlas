using Atlas.Entities;
using Atlas.Enums;
using Atlas.Events;
using Atlas.EventSystem;
using Scp914;

using HarmonyLib;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(Scp914Controller), nameof(Scp914Controller.ServerInteract))]
    public class ActivatingScp914Patch
    {
        public static bool Prefix(Scp914Controller __instance, ReferenceHub ply, byte colliderId)
        {
            try
            {
				if (__instance._remainingCooldown > 0f)
					return false;

				Player player = PlayersList.Get(ply);

				if (player == null)
					return true;

				if (colliderId == 0)
				{
					__instance._remainingCooldown = ConfigHolder.Scp914.KnobChangeCooldown;

					ChangingKnob ev = EventManager.Invoke(new ChangingKnob(player, Map.Scp914.Setting, Map.Scp914.NewSetting, true));

					if (!ev.IsAllowed)
						return false;

					__instance.Network_knobSetting = (Scp914KnobSetting)ev.New;

					if (ConfigHolder.Scp914.PlaySoundOnKnobChange)
						__instance.RpcPlaySound(0);

					return false;
				}

				if (colliderId != 1)
					return false;

				ActivatingScp914 aEv = EventManager.Invoke(new ActivatingScp914(player, __instance, true));

				if (!aEv.IsAllowed)
					return false;

				__instance._remainingCooldown = __instance._totalSequenceTime;
				__instance._isUpgrading = true;
				__instance._itemsAlreadyUpgraded = false;

				if (ConfigHolder.Scp914.PlaySoundOnActivation)
					__instance.RpcPlaySound(1);

				return false;
            }
            catch (Exception e)
            {
                Manager.Exc<ActivatingScp914Patch>(e);

                return true;
            }
        }
    }
}
