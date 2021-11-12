using Atlas.Events;
using Atlas.EventSystem;
using Atlas.Extensions;

using HarmonyLib;

using System;
using System.Collections.Generic;

namespace Atlas.Patches.Events
{
    [HarmonyPatch(typeof(NineTailedFoxAnnouncer), nameof(NineTailedFoxAnnouncer.AnnounceScpTermination))]
    public class AnnouncingScpTerminationPatch
    {
        public static bool Prefix(Role scp, PlayerStats.HitInfo hit, string groupId)
        {
            try
            {
				NineTailedFoxAnnouncer.singleton.scpListTimer = 0f;

				AnnouncingScpTermination ev = EventManager.Invoke(new AnnouncingScpTermination(hit.GetKiller(), scp, hit, true));

				if (!ev.IsAllowed)
					return false;

				hit = ev.HitInfo;

				if (!string.IsNullOrEmpty(groupId))
				{
					foreach (NineTailedFoxAnnouncer.ScpDeath scpDeath in NineTailedFoxAnnouncer.scpDeaths)
					{
						if (scpDeath.group == groupId)
						{
							scpDeath.scpSubjects.Add(scp);

							return false;
						}
					}
				}

				NineTailedFoxAnnouncer.scpDeaths.Add(new NineTailedFoxAnnouncer.ScpDeath
				{
					scpSubjects = new List<Role>(new Role[]
					{
						scp
					}),

					group = groupId,
					hitInfo = hit
				});

				return false;
            }
            catch (Exception e)
            {
                Manager.Exc<AnnouncingScpTerminationPatch>(e);

                return true;
            }
        }
    }
}
