using UnityEngine;

using Mirror;

using Atlas.Extensions;
using Atlas.Abstractions;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for <see cref="BreakableWindow"/>.
    /// </summary>
    public class Window : NetworkObject
    {
        internal BreakableWindow window;

        private bool colliders;

        /// <summary>
        /// Initialzes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="window">The original window.</param>
        public Window(BreakableWindow window, bool addToApi = false)
        {
            this.window = window;

            if (addToApi)
                Map.windows.Add(this);
        }

        /// <summary>
        /// Gets the window's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public override GameObject GameObject { get => window.gameObject; }

        /// <summary>
        /// Gets the window's network ID.
        /// </summary>
        public override uint NetId { get => window.netId; }

        /// <summary>
        /// Gets or sets a boolean indicating whether the window is broken or not.
        /// </summary>
        public bool IsBroken
        {
            get => window.isBroken;
            set
            {
                if (value)
                    Break();

                Status = new WindowStatus
                {
                    IsBroken = value,
                    Position = Position,
                    Rotation = Rotation
                };
            }
        }

        /// <summary>
        /// Get or sets a boolean indicating whether the colliders are active or not.
        /// </summary>
        public bool Colliders
        {
            get => colliders;
            set
            {
                colliders = value;

                if (value)
                    window.EnableColliders();
                else
                    foreach (Collider collider in window.GetComponentsInChildren<Collider>())
                        collider.enabled = false;
            }
        }

        /// <summary>
        /// Gets or sets the window's health.
        /// </summary>
        public float Health { get => window.health; set => window.health = value; }

        /// <summary>
        /// Gets or sets the window's position.
        /// </summary>
        public override Vector3 Position
        {
            get => window.transform.position;
            set
            {
                NetworkServer.UnSpawn(GameObject);

                Status = new WindowStatus
                {
                    IsBroken = IsBroken,
                    Position = value,
                    Rotation = Rotation
                };

                window.transform.position = value;

                NetworkServer.Spawn(GameObject);
            }
        }

        /// <summary>
        /// Gets or sets the window's position.
        /// </summary>
        public override Vector3 Scale
        {
            get => window.transform.localScale;
            set
            {
                NetworkServer.UnSpawn(GameObject);

                Status = new WindowStatus
                {
                    IsBroken = IsBroken,
                    Position = Position,
                    Rotation = Rotation
                };

                window.transform.localScale = value;

                NetworkServer.Spawn(GameObject);
            }
        }

        /// <summary>
        /// Gets or sets the window's rotation.
        /// </summary>
        public override Quaternion Rotation
        {
            get => window.transform.rotation;
            set
            {
                NetworkServer.UnSpawn(GameObject);

                Status = new WindowStatus
                {
                    IsBroken = IsBroken,
                    Position = Position,
                    Rotation = value
                };

                window.transform.rotation = Rotation;

                NetworkServer.Spawn(GameObject);
            }
        }

        /// <summary>
        /// Gets or sets the window's status.
        /// </summary>
        public WindowStatus Status { get => new WindowStatus(window.NetworksyncStatus); set => window.NetworksyncStatus = value.ToBase(); }

        /// <summary>
        /// Damages the window.
        /// </summary>
        /// <param name="damage">The amount of damage to deal. If it is less than the window's health, it will be broken.</param>
        public void Damage(float damage)
        {
            window.health -= damage;

            if (window.health < 0f)
                Break();
        }

        /// <summary>
        /// Breaks the window.
        /// </summary>
        public void Break()
            => window.StartCoroutine(window.BreakWindow());

        /// <summary>
        /// Deletes the window.
        /// </summary>
        public override void Delete()
            => GameObject.Delete();

        /// <summary>
        /// Tries to get a <see cref="Window"/> from a <see cref="BreakableWindow"/>. This method will ALWAYS return an instance as it creates a new item in case it wasn't found.
        /// </summary>
        /// <param name="window">The window to find.</param>
        /// <returns>The window found, if any.</returns>
        public static Window Get(BreakableWindow window)
        {
            foreach (Window w in Map.windows)
            {
                if (w.NetId == window.netId || w.window == window)
                    return w;
            }

            return new Window(window, true);
        }
    }
}