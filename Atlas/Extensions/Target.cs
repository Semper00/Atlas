using System.Collections.Generic;

using Atlas.Enums;
using Atlas.Entities;

namespace Atlas.Extensions
{

    public static class TargetExtensions
    {
        internal static readonly IReadOnlyDictionary<string, TargetType> Types = new Dictionary<string, TargetType>
        {
            ["sportTargetPrefab"] = TargetType.SportTarget,
            ["dboyTargetPrefab"] = TargetType.ClassDTarget,
            ["binaryTargetPrefab"] = TargetType.BinaryTarget,
        };

        internal static readonly IReadOnlyDictionary<TargetType, string> Prefabs = new Dictionary<TargetType, string>
        {
            [TargetType.BinaryTarget] = "binaryTargetPrefab",
            [TargetType.ClassDTarget] = "dboyTargetPrefab",
            [TargetType.SportTarget] = "sportTargetPrefab"
        };

        public static TargetType GetTargetType(this Target target)
        {
            if (target.GameObject.name.StartsWith("sport"))
                return TargetType.SportTarget;

            if (target.GameObject.name.StartsWith("dboy"))
                return TargetType.ClassDTarget;

            if (target.GameObject.name.StartsWith("binary"))
                return TargetType.BinaryTarget;

            return TargetType.BinaryTarget;
        }

        public static string GetPrefabName(this TargetType type)
            => Prefabs[type];

        public static TargetType GetPrefabTargetType(this string str)
            => Types.TryGetValue(str, out TargetType type) ? type : TargetType.BinaryTarget;
    }
}
