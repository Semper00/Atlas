using Atlas.Events;
using Atlas.EventSystem;
using Atlas.Commands;
using Atlas.Enums;

using HarmonyLib;

using System;

using RemoteAdmin;

namespace Atlas.Patches.Features
{
    [HarmonyPatch(typeof(CommandProcessor), nameof(CommandProcessor.ProcessQuery))]
    public class TriggerCommandPatch
    {
        public static bool Prefix(ref string query, CommandSender sender)
        {
            try
            {
                SendingCommand ev = EventManager.Invoke(new SendingCommand(query, sender, CommandSource.RemoteAdmin, true));

                if (!ev.IsAllowed)
                {
                    sender.RaReply($"ATLAS#Your command was disallowed by a server modification", false, true, string.Empty);

                    return false;
                }

                query = ev.Query;

                if (CommandManager.ExecuteCommand(ev.Source, ev.Player, ev.Query))
                {
                    Log.DebugFeature<CommandManager>($"Succesfully executed command: {query}");

                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Manager.Exc<TriggerCommandPatch>(e);

                return true;
            }
        }
    }
}
