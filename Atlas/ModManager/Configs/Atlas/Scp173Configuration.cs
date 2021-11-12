using System.ComponentModel;
using System.Collections.Generic;

using Atlas.Interfaces;

namespace Atlas.ModManager.Configs.Atlas
{
    public class Scp173Configuration : IConfig
    {
        public List<RoleType> DisallowedRoles { get; set; } = new List<RoleType>() { RoleType.Tutorial };

        public bool DisableBypass { get; set; } = false;
        public bool DisableGodMode { get; set; } = false;
    }
}
