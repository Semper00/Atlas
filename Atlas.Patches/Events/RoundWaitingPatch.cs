using Atlas.Events;
using Atlas.EventSystem;

using HarmonyLib;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public class RoundWaitingPatch
    {
        public static void Postfix(string q, ConsoleColor color = ConsoleColor.Gray)
        {
            if (q.StartsWith("Waiting for players..."))
                EventManager.Invoke(new RoundWaiting());
        }
    }
}
