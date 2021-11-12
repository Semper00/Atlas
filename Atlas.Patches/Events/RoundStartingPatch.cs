using Atlas.Events;
using Atlas.EventSystem;

using HarmonyLib;

using System;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(CharacterClassManager), nameof(CharacterClassManager.ForceRoundStart))]
    public class RoundStartingPatch
    {
        public static bool Prefix(ref bool __result)
        {
			OneOhSixContainer.used = false;

			ServerLogs.AddLog(ServerLogs.Modules.Logger, "Round has been started.", global::ServerLogs.ServerLogType.GameEvent, false);
			ServerConsole.AddLog("New round has been started.", ConsoleColor.Gray);

			GameCore.RoundStart.singleton.NetworkTimer = -1;
			GameCore.RoundStart.RoundStartTimer.Restart();

			EventManager.Invoke(new RoundStarting());

			__result = true;

			return false;
		}
    }
}