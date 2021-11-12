using Atlas.Entities;

namespace Atlas.Extensions
{
    public static class HitExtensions
    {
        public static Player GetKiller(this PlayerStats.HitInfo hit)
            => hit.Tool == DamageTypes.Grenade ? PlayersList.Get(hit.PlayerId) : PlayersList.Get(hit.RHub);
    }
}
