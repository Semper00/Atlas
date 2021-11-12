using Interactables.Interobjects.DoorUtils;

using MapGeneration.Distributors;

using InventorySystem.Items;
using InventorySystem.Items.Pickups;

using Atlas.Entities;
using Atlas.Entities.Pickups.Base;
using Atlas.Entities.Items.Base;

namespace Atlas.Extensions
{
    public static class MapExtensions
    {
        public static int GetId(this LockerChamber chamber)
            => chamber.GetInstanceID();

        /// <summary>
        /// An extension of <see cref="Camera.Get(Camera079)"/>.
        /// </summary>
        public static Camera Get(this Camera079 cam)
            => Camera.Get(cam);

        /// <summary>
        /// An extension of <see cref="Camera.Get(Enums.CameraType)"/>.
        /// </summary>
        public static Camera Get(this Enums.CameraType type)
            => Camera.Get(type);

        /// <summary>
        /// An extension of <see cref="Chamber.Get(LockerChamber)"/>.
        /// </summary>
        public static Chamber Get(this LockerChamber cham)
            => Chamber.Get(cham);

        /// <summary>
        /// An extension of <see cref="Entities.Door.Get(DoorVariant)"/>.
        /// </summary>
        public static Door Get(this DoorVariant door)
            => Door.Get(door);

        /// <summary>
        /// An extension of <see cref="Entities.Item.Get(BasePickup)"/>.
        /// </summary>
        public static BaseItem Get(this ItemBase pickup)
            => BaseItem.Get(pickup);

        public static BaseItem GetItem(this ItemPickupBase pickup)
            => BaseItem.Get(Server.Host.Inventory.CreateItemInstance(pickup.Info.ItemId, false));

        public static BasePickup Get(this ItemPickupBase pickupBase)
            => BasePickup.Get(pickupBase);

        /// <summary>
        /// An extension of <see cref="Locker.Get(Locker)"/>.
        /// </summary>
        public static Entities.Locker Get(this MapGeneration.Distributors.Locker locker)
            => Entities.Locker.Get(locker);

        /// <summary>
        /// An extension of <see cref="Entities.Ragdoll.Get(Ragdoll)"/>.
        /// </summary>
        public static Entities.Ragdoll Get(this Ragdoll ragdoll)
            => Entities.Ragdoll.Get(ragdoll);


        /// <summary>
        /// An extension of <see cref="Tesla.Get(TeslaGate)"/>.
        /// </summary>
        public static Tesla Get(this TeslaGate gate)
            => Tesla.Get(gate);

        /// <summary>
        /// An extension of <see cref="Window.Get(BreakableWindow)"/>.
        /// </summary>
        public static Window Get(this BreakableWindow window)
            => Window.Get(window);

        /*
        /// <summary>
        /// An extension of <see cref="Workstation.Get(WorkStation)"/>.
        /// </summary>
        public static Workstation Get(this WorkStation station)
            => Workstation.Get(station);
        */
    }
}
