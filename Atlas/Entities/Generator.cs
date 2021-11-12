using UnityEngine;

using Interactables.Interobjects.DoorUtils;

using MapGeneration.Distributors;

using Atlas.Enums;
using Atlas.Extensions;
using Atlas.Abstractions;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for the <see cref="Scp079Generator"/> class.
    /// </summary>
    public class Generator : NetworkObject
    {
        internal Scp079Generator gen;

        /// <summary>
        /// Initialzes a new instance of the <see cref="Generator"/> class.
        /// </summary>
        /// <param name="gen"></param>
        internal Generator(Scp079Generator gen, bool addToApi = false)
        {
            this.gen = gen;

            Room = Room.Get(gen.gameObject);

            if (addToApi)
                Map.generators.Add(this);
        }

        /// <summary>
        /// Gets the generator's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public override GameObject GameObject { get => gen.gameObject; }

        /// <summary>
        /// Gets the generator's room.
        /// </summary>
        public Room Room { get; }

        /// <summary>
        /// Gets or sets the generator's position.
        /// </summary>
        public override Vector3 Position { get => GameObject.transform.position; set => GameObject.Teleport(value); }

        /// <inheritdoc/>
        public override Vector3 Scale { get => GameObject.transform.localScale; set => GameObject.Resize(value); }

        /// <summary>
        /// Gets or sets the generator's rotation.
        /// </summary>
        public override Quaternion Rotation { get => GameObject.transform.rotation; set => GameObject.Rotate(value); }

        /// <summary>
        /// Gets or sets the generator's required keycard permissions.
        /// </summary>
        public KeycardPermissions Permissions { get => gen._requiredPermission; set => gen._requiredPermission = value; }

        /// <summary>
        /// Gets the generator flags.
        /// </summary>
        public GeneratorFlags Flags { get => (GeneratorFlags)gen.Network_flags; set => gen.Network_flags = (byte)value; }

        /// <summary>
        /// Gets the generator's network ID.
        /// </summary>
        public override uint NetId { get => gen.netId; }

        /// <summary>
        /// Gets or sets a value indicating whether the generator has engaged or not.
        /// </summary>
        public bool Engaged { get => gen.Engaged; set => gen.Engaged = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the generator is currently activating or not.
        /// </summary>
        public bool Activating { get => gen.Activating; set => gen.Activating = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the generator is ready for activation or not.
        /// </summary>
        public bool Ready { get => gen.ActivationReady; }

        /// <summary>
        /// Gets or sets a boolean indicating whether the door is unlocked or not.
        /// </summary>
        public bool IsUnlocked
        {
            get => Flags.HasFlag(GeneratorFlags.Unlocked);
            set
            {
                if (value)
                    Flags |= GeneratorFlags.Unlocked;
                else
                    Flags &= GeneratorFlags.Unlocked;
            }
        }

        /// <summary>
        /// Gets or sets a boolean indicating whether the door is open or not.
        /// </summary>
        public bool IsOpen
        {
            get => Flags.HasFlag(GeneratorFlags.Open);
            set
            {
                if (value)
                    Flags |= GeneratorFlags.Open;
                else
                    Flags &= GeneratorFlags.Open;
            }
        }

        /// <summary>
        /// Creates an interaction.
        /// </summary>
        /// <param name="player">The player trying to interact.</param>
        /// <param name="op">The interaction.</param>
        public void Interact(Player player)
            => gen.ServerInteract(player.Hub, 0);

        /// <summary>
        /// Unlocks this generator.
        /// </summary>
        public void Unlock()
            => IsUnlocked = true;

        /// <summary>
        /// Forces an overcharge (turns off all lights).
        /// </summary>
        /// <param name="time">The time to turn the lights off for.</param>
        /// <param name="doors">Whether or not to lock and close doors too. <b>Keep in mind that doors will NOT unlock automatically, you HAVE to do it manually.</b></param>
        public void Overcharge(float time = 10f, bool doors = false)
            => Map.Overcharge(time, doors);

        /// <summary>
        /// Deletes this generator.
        /// </summary>
        public override void Delete()
            => gen.gameObject.Delete();

        /// <summary>
        /// Tries to get a <see cref="Generator"/> from a <see cref="Generator079"/>. This method will ALWAYS return an instance as it creates a new item in case it wasn't found.
        /// </summary>
        /// <param name="gen"></param>
        /// <returns></returns>
        public static Generator Get(Scp079Generator gen)
        {
            foreach (Generator generator in Map.generators)
            {
                if (generator.gen == gen || generator.NetId == gen.netId)
                    return generator;
            }

            return new Generator(gen, true);
        }
    }
}