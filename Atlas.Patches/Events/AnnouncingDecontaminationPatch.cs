using Atlas.Events;
using Atlas.EventSystem;

using System;
using LightContainmentZoneDecontamination;
using UnityEngine;

using HarmonyLib;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(DecontaminationController), nameof(DecontaminationController.UpdateSpeaker))]
    public class AnnouncingDecontaminationPatch
    {
        public static bool Prefix(DecontaminationController __instance, bool hard)
        {
            try
            {
                float b = 0f;

                if (__instance._curFunction == DecontaminationController.DecontaminationPhase.PhaseFunction.Final 
                    || __instance._curFunction == DecontaminationController.DecontaminationPhase.PhaseFunction.GloballyAudible)
                    b = 1f;
                else if (Mathf.Abs(SpectatorCamera.Singleton.cam.transform.position.y) < 200f)
                    b = 1f;

                AnnouncingDecontamination ev = EventManager.Invoke(new AnnouncingDecontamination((int)__instance._curFunction, hard));

                hard = ev.Global;

                __instance._curFunction = (DecontaminationController.DecontaminationPhase.PhaseFunction)ev.Id;

                __instance.AnnouncementAudioSource.volume = Mathf.Lerp(__instance.AnnouncementAudioSource.volume, b, hard ? 1f : (Time.deltaTime * 4f));

                return false;
            }
            catch (Exception e)
            {
                Manager.Exc<AnnouncingDecontaminationPatch>(e);

                return true;
            }
        }
    }
}
