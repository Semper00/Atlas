using Atlas.Events;
using Atlas.EventSystem;
using HarmonyLib;

using System;

using Respawning;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(RespawnEffectsController), nameof(RespawnEffectsController.PlayCassieAnnouncement))]
    public class AnnouncingPatch
    {
        public static bool Prefix(string words, bool makeHold, bool makeNoise)
        {
            try
            {
                var ctrls = RespawnEffectsController.AllControllers;

                Announcing ev = EventManager.Invoke(new Announcing(words, makeHold, makeNoise, true));

                if (!ev.IsAllowed)
                    return false;

                for (int i = 0; i < ctrls.Count; i++)
                {
                    var ctrl = ctrls[i];

                    if (ctrl == null)
                    {
                        RespawnEffectsController.AllControllers.RemoveAt(i);
                        continue;
                    }

                    ctrl.ServerPassCassie(ev.Words, ev.IsHeld, ev.MakeNoise);
                }

                return false;
            }
            catch (Exception e)
            {
                Manager.Exc<AnnouncingPatch>(e);

                return true;
            }
        }
    }
}
