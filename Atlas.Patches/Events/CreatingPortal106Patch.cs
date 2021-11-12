using Atlas.Events;
using Atlas.EventSystem;
using Atlas.Entities;

using HarmonyLib;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(Scp106PlayerScript), nameof(Scp106PlayerScript.CreatePortalInCurrentPosition))]
    public class CreatingPortal106Patch
    {

    }
}
