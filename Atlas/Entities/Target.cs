using System.Collections.Generic;

using UnityEngine;

using InventorySystem.Items.Firearms.Utilities;

using Mirror;

using Atlas.Abstractions;
using Atlas.Extensions;
using Atlas.Enums;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper for the <see cref="ShootingTarget"/> class.
    /// </summary>
    public class Target : NetworkObject
    {
        internal ShootingTarget _target;

        public Target(ShootingTarget _target, bool addToApi = false)
        {
            this._target = _target;

            if (addToApi)
                Map.targets.Add(this);
        }

        /// <inheritdoc/>
        public override GameObject GameObject { get => _target.gameObject; }

        /// <summary>
        /// Gets or sets the target's type.
        /// </summary>
        public TargetType Type
        {
            get => this.GetTargetType();
            set
            {
                if (value == Type)
                    return;

                var pos = Position;
                var rot = Rotation;
                var scale = Scale;
                var type = Type;
                var maxhp = MaxHealth;
                var hp = Health;
                var hits = Hits;

                NetworkServer.Destroy(GameObject);

                _target = SpawnBase(type, pos, scale, rot);

                _target._maxHp = maxhp;
                _target._hp = hp;
                _target._hits?.Clear();
                _target._hits?.AddRange(hits);
            }
        }

        /// <summary>
        /// Gets or sets a list of all hits so far.
        /// </summary>
        public List<GameObject> Hits
        {
            get => _target._hits;
            set
            {
                _target._hits.Clear();
                _target._hits.AddRange(value);
            }
        }

        /// <summary>
        /// Gets the position of the target's center.
        /// </summary>
        public Vector3 Center { get => _target.CenterOfMass; set => _target._bullsEye.position = value; }

        /// <inheritdoc/>
        public override Vector3 Position { get => _target.transform.position; set => _target.gameObject.Teleport(value); }

        /// <inheritdoc/>
        public override Vector3 Scale { get => _target.transform.localScale; set => _target.gameObject.Resize(value); }

        /// <inheritdoc/>
        public override Quaternion Rotation { get => _target.transform.rotation; set => _target.gameObject.Rotate(value); }

        /// <inheritdoc/>
        public override uint NetId { get => _target.netId; }

        /// <summary>
        /// Gets or sets the average hit amount.
        /// </summary>
        public float Average { get => _target._avg; set => _target._avg = value; }

        /// <summary>
        /// Gets or sets the target's health.
        /// </summary>
        public float Health { get => _target._hp; set => _target._hp = value; }

        /// <summary>
        /// Gets or sets the target's max health.
        /// </summary>
        public int MaxHealth { get => _target._maxHp; set => _target._maxHp = value; }

        /// <summary>
        /// Gets or sets the target's automatic destruction time.
        /// </summary>
        public int AutoDestroyTime { get => _target._autoDestroyTime; set => _target._autoDestroyTime = value; }

        /// <summary>
        /// Gets or sets the target's sync mode - setting this to true will make all players receive an update message of this target when damaged.
        /// </summary>
        public bool SyncMode { get => _target.Network_syncMode; set => _target.Network_syncMode = value; }

        /// <summary>
        /// Damages the target.
        /// </summary>
        /// <param name="damage">The damage to deal.</param>
        /// <param name="hitPosition">The position of the hit.</param>
        /// <returns></returns>
        public bool Damage(float damage, InventorySystem.Items.IDamageDealer damageDealer = default, Vector3? hitPosition = null)
            => _target.Damage(damage, damageDealer, new Footprinting.Footprint(PlayersList.Host.Hub), hitPosition.HasValue ? hitPosition.Value : Position);

        /// <inheritdoc/>
        public override void Delete()
            => _target.gameObject.Delete();

        /// <summary>
        /// Clears target's hits and resets it's HP.
        /// </summary>
        public void Clear()
            => _target.ClearTarget();

        /// <summary>
        /// Forces the target to "use a button". Doing so will trigger the action in the enum's description.
        /// </summary>
        /// <param name="type">The button to use.</param>
        public void Do(TargetButtonType type)
        {
            if (type == TargetButtonType.GlobalResults)
                return;

            if (type == TargetButtonType.Remove)
            {
                Delete();

                return;
            }

            _target.UseButton((ShootingTarget.TargetButton)type);
        }

        /// <summary>
        /// Sends an update message.
        /// </summary>
        public void RpcSendInfo()
            => _target.RpcSendInfo(MaxHealth, AutoDestroyTime);

        /// <summary>
        /// Sends an update message.
        /// </summary>
        /// <param name="maxHealth">The target's health.</param>
        /// <param name="autoDestroyTime">The target's destroy time.</param>
        public void RpcSendInfo(int maxHealth, int autoDestroyTime)
            => _target.RpcSendInfo(maxHealth, autoDestroyTime);

        /// <summary>
        /// Sends an update message to all players while dealing damage.
        /// </summary>
        /// <param name="damage">The damage dealt.</param>
        /// <param name="distance">The distance of the hit.</param>
        /// <param name="pos">The position of the hit.</param>
        public void Damage(float damage, float distance = 0f, Vector3? pos = null)
        {
            foreach (Player player in PlayersList.players.Values)
            {
                _target.UserCode_TargetRpcReceiveData(player.Connection, damage, distance, pos.HasValue ? pos.Value : Position);
            }
        }

        /// <summary>
        /// Spawns a <see cref="ShootingTarget"/>.
        /// </summary>
        /// <param name="type">The type to spawn.</param>
        /// <param name="pos">Position to spawn at.</param>
        /// <param name="scale">Scale to spawn at.</param>
        /// <param name="rot">Rotation to spawn at.</param>
        /// <returns></returns>
        public static ShootingTarget SpawnBase(TargetType type, Vector3 pos, Vector3 scale, Quaternion rot)
        {
            return Prefab.GetPrefab(Prefab.GetPrefabType(type.GetPrefabName())).Spawn<ShootingTarget>(pos, scale, rot, true);
        }

        /// <summary>
        /// Spawns a shooting target.
        /// </summary>
        /// <param name="type">The type to spawn.</param>
        /// <param name="position">The position to spawn at.</param>
        /// <param name="scale">The scale to use.</param>
        /// <param name="rotation">The rotation to spawn with.</param>
        /// <returns></returns>
        public static Target Spawn(TargetType type, Vector3 position, Vector3 scale, Quaternion rotation)
        {
            var copy = SpawnBase(type, position, scale, rotation);

            if (copy != null)
                return new Target(copy, true);

            return null;
        }

        /// <summary>
        /// Gets a <see cref="Target"/> from a <see cref="ShootingTarget"/>.
        /// </summary>
        /// <param name="target">The instance of the found target.</param>
        /// <returns></returns>
        public static Target Get(ShootingTarget target)
        {
            foreach (var tt in Map.targets)
            {
                if (tt.NetId == target.netId)
                    return tt;
            }

            return new Target(target, true);
        }
    }
}