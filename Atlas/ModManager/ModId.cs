namespace Atlas.ModManager
{
#pragma warning disable CS0659
    public class ModId
#pragma warning restore CS0659
    {
        public string AssemblyName { get; set; }
        public string AssemblyLocation { get; set; }
        public string AssemblyVersion { get; set; }

        public string ModName { get; set; }
        public string ModVersion { get; set; }

        public bool HasChanged(ModId other)
            => AssemblyName != other.AssemblyName || AssemblyLocation != other.AssemblyLocation ||
               AssemblyVersion != other.AssemblyVersion || ModName != other.ModName || ModVersion != other.ModVersion;

        public override bool Equals(object obj)
        {
            ModId other = Functions.As<ModId>(obj);

            if (other == null)
                return base.Equals(obj);

            return !HasChanged(other);
        }
    }
}
