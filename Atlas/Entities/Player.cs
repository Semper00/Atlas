using System;
using System.Collections.Generic;
using System.Linq;

using Mirror;
using Mirror.LiteNetLib4Mirror;
using MEC;

using UnityEngine;

using Hints;
using RemoteAdmin;
using PlayableScps;
using CustomPlayerEffects;
using NorthwoodLib.Pools;

using Atlas.Enums;
using Atlas.Extensions;
using Atlas.Exceptions;
using Atlas.Entities.Items;
using Atlas.Entities.Pickups.Base;
using Atlas.Entities.Items.Base;
using Atlas.Toolkit;

using InventorySystem;
using InventorySystem.Disarming;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;

using Assets._Scripts.Dissonance;

namespace Atlas.Entities
{
    // A lot of stuff may not work cuz NW broke everything

    /// <summary>
    /// A nice little wrapper around the <see cref="ReferenceHub"/> class to simplify interacting with players.
    /// </summary>
    public class Player
    {
        internal RoleType? appearance;

        internal readonly List<BaseItem> items;
        internal readonly List<BasePickup> droppedItems;
        internal readonly List<Ragdoll> ragdolls;
        internal readonly List<RoleType> roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class by it's <see cref="ReferenceHub"/>.
        /// </summary>
        /// <param name="hub">The player's <see cref="ReferenceHub"/></param>
        internal Player(ReferenceHub hub)
        {
            if (hub == null)
                throw new HubNotFoundException("hub");

            Hub = hub;
            HintDisplay = Hub.hints;
            Inventory = Hub.inventory;
            PlayerCamera = Hub.PlayerCameraReference;
            Dissonance = Hub.dissonanceUserSetup;
            Radio = Hub.radio;

            Session = new Container();
            Rank = new Rank(this);

            var icom = hub.GetComponent<global::Intercom>();

            if (icom == null)
                Log.Error($"The Intercom component of player {UserId} is null!");

            if (icom != null)
                Intercom = new Intercom(icom);

            if (Broadcaster == null)
                Broadcaster = IsHost ? ReferenceHub.HostHub?.GetComponent<Broadcast>() : Server.Host?.Broadcaster;

            items = ListPool<BaseItem>.Shared.Rent();
            droppedItems = ListPool<BasePickup>.Shared.Rent();
            roles = ListPool<RoleType>.Shared.Rent();
            ragdolls = ListPool<Ragdoll>.Shared.Rent();

            Ammo = AmmoManager.GetAmmoManager(this);
        }

        ~Player()
        {
            ListPool<BaseItem>.Shared.Return(items);
            ListPool<BasePickup>.Shared.Return(droppedItems);
            ListPool<Ragdoll>.Shared.Return(ragdolls);
            ListPool<RoleType>.Shared.Return(roles);
        }

        /// <summary>
        /// Gets the player's <see cref="ReferenceHub"/>.
        /// </summary>
        public ReferenceHub Hub { get; }

        /// <summary>
        /// Gets the player's <see cref="global::Radio"/> component.
        /// </summary>
        public Radio Radio { get; }

        /// <summary>
        /// Get the player's <see cref="DissonanceUserSetup"/>.
        /// </summary>
        public DissonanceUserSetup Dissonance { get; }

        /// <summary>
        /// Gets the player's <see cref="GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => Hub.gameObject; }

        /// <summary>
        /// Gets the player's <see cref="InventorySystem.Inventory"/>.
        /// </summary>
        public Inventory Inventory { get; }

        /// <summary>
        /// Gets the player's ammo manager.
        /// </summary>
        public AmmoManager Ammo { get; }

        /// <summary>
        /// Gets the player's <see cref="global::Broadcast"/>.
        /// </summary>
        public Broadcast Broadcaster { get; }

        /// <summary>
        /// Gets the player's <see cref="HintDisplay"/>.
        /// </summary>
        public HintDisplay HintDisplay { get; }

        /// <summary>
        /// Gets or sets the player's current SCP.
        /// </summary>
        public PlayableScp Scp { get => Hub.scpsController.CurrentScp; set => Hub.scpsController.CurrentScp = value; }

        /// <summary>
        /// Gets the player's camera transform.
        /// </summary>
        public Transform PlayerCamera { get; }

        /// <summary>
        /// Gets the player's Intercom component. <b>May be null.</b>
        /// </summary>
        public Intercom Intercom { get; }

        /// <summary>
        /// Tries to find the room the player is located inside. Will default to the surface room if not found.
        /// </summary>
        public Room Room => Room.Get(GameObject);

        /// <summary>
        /// Gets the player's <see cref="NetworkIdentity"/>.
        /// </summary>
        public NetworkIdentity Identitity => Hub.networkIdentity;

        /// <summary>
        /// Gets the player's <see cref="NetworkConnection"/>.
        /// </summary>
        public NetworkConnection Connection => Hub.characterClassManager.Connection;

        /// <summary>
        /// Gets a read-only list of all roles of this player.
        /// </summary>
        public IReadOnlyList<RoleType> Roles { get => roles; }

        /// <summary>
        /// Gets a read-only list of doors that this player has locked. 
        /// </summary>
        public IReadOnlyList<Door> LockedDoors { get => Door.Get(Hub.scp079PlayerScript.lockedDoors); }

        /// <summary>
        /// Gets a read-only list of items in this player's inventory.
        /// </summary>
        public IReadOnlyList<BaseItem> Items { get => items; }

        /// <summary>
        /// Gets a read-only list of all ragdolls owned by this player.
        /// </summary>
        public IReadOnlyList<Ragdoll> Ragdolls { get => ragdolls; }

        /// <summary>
        /// Gets a read-only list of owned firearms.
        /// </summary>
        public IReadOnlyList<FirearmItem> Firearms { get => items.Where(x => x is FirearmItem).Select(x => x as FirearmItem).ToList(); }

        /// <summary>
        /// Gets or sets the player's SCP-079 abilities. May be null.
        /// </summary>
        public Scp079PlayerScript.Ability079[] Abilities { get => Hub.scp079PlayerScript?.abilities ?? null; set => Hub.scp079PlayerScript.abilities = value; }

        /// <summary>
        /// Gets or sets the player's SCP-079 levels. May be null.
        /// </summary>
        public Scp079PlayerScript.Level079[] Levels { get => Hub.scp079PlayerScript?.levels; set => Hub.scp079PlayerScript.levels = value; }

        /// <summary>
        /// Gets or sets the player's current item.
        /// </summary>
        public BaseItem CurrentItem
        {
            get => BaseItem.Get(Inventory.CurInstance);
            set
            {
                if (value == null || value.Id == ItemType.None)
                    Inventory.ServerSelectItem(0);
                else
                    if (!Inventory.UserInventory.Items.TryGetValue(value.Serial, out _))
                {
                    AddItem(value);

                    Timing.CallDelayed(0.5f, () => Inventory.ServerSelectItem(value.Serial));
                }
            }
        }

        /// <summary>
        /// Gets or sets the player's current item.
        /// </summary>
        public ItemType CurrentItemId { get => CurrentItem?.Id ?? ItemType.None; 
            set => CurrentItem = new BaseItem(value); }

        /// <summary>
        /// Gets a <see cref="HashSet{T}"/> of players that this player can't see.
        /// </summary>
        public HashSet<Player> TargetGhosts { get; }

        /// <summary>
        /// Gets the player's current <see cref="Entities.Camera"/>, or null if this player is not SCP-079.
        /// </summary>
        public Camera Camera { get; }

        /// <summary>
        /// Gets the player's <see cref="Container"/>.
        /// </summary>
        public Container Session { get; }

        /// <summary>
        /// Gets the player's <see cref="PlayerCommandSender"/>.
        /// </summary>
        public PlayerCommandSender Sender { get => Hub.queryProcessor._sender; }

        /// <summary>
        /// Gets the player's <see cref="Entities.Rank"/>.
        /// </summary>
        public Rank Rank { get; }

        /// <summary>
        /// Gets or sets the player's current voice profile.
        /// </summary>
        public VoiceProfile VoiceProfile { get => Dissonance._currentProfile; set => Dissonance.SetProfile(value); }

        /// <summary>
        /// Gets or sets the player that cuffed this player.
        /// </summary>
        public Player Cuffer
        {
            get
            {
                foreach (DisarmedPlayers.DisarmedEntry disarmed in DisarmedPlayers.Entries)
                {
                    if (PlayersList.Get(disarmed.DisarmedPlayer) == this)
                        return PlayersList.Get(disarmed.Disarmer);
                }

                return null;
            }
            set
            {
                for (int i = 0; i < DisarmedPlayers.Entries.Count; i++)
                {
                    if (DisarmedPlayers.Entries[i].DisarmedPlayer == Inventory.netId)
                    {
                        DisarmedPlayers.Entries.RemoveAt(i);
                        break;
                    }
                }

                if (value != null)
                    Inventory.SetDisarmedStatus(value.Inventory);
            }
        }

        /// <summary>
        /// Gets or sets the player's current role.
        /// </summary>
        public RoleType Role { get => Hub.characterClassManager.NetworkCurClass; set => ChangeRole(value); }

        /// <summary>
        /// Gets or sets the role the player appears as while having a different role.
        /// </summary>
        public RoleType Appearance 
        { 
            get => appearance.HasValue ? appearance.Value : Role; 
            set
            {
                this.ChangeAppearance(value);
            }
        }

        /// <summary>
        /// Gets the player's current <see cref="global::Team"/>.
        /// </summary>
        public Team Team { get => Role.GetTeam(); }

        /// <summary>
        /// Gets the player's current <see cref="global::Fraction"/>.
        /// </summary>
        public Faction Fraction { get => Hub.characterClassManager.Faction; }

        /// <summary>
        /// Gets the player's leading team.
        /// </summary>
        public LeadingTeam LeadingTeam { get => Team.GetLeadingTeam(); }

        /// <summary>
        /// Gets or sets the player's info area. You can hide all those annoying elements if you want to.
        /// </summary>
        public PlayerInfoArea InfoArea { get => Hub.nicknameSync.Network_playerInfoToShow; set => Hub.nicknameSync.Network_playerInfoToShow = value; }

        /// <summary>
        /// Gets the player's current movement state.
        /// </summary>
        public PlayerMovementState MoveState { get => Hub.animationController.MoveState; }

        /// <summary>
        /// Gets or sets this player's current position.
        /// </summary>
        public Vector3 Position { get => Hub.playerMovementSync.GetRealPosition(); set => Hub.playerMovementSync.OverridePosition(value, 0f); }

        /// <summary>
        /// Gets or sets this player's current rotation.
        /// </summary>
        public Vector3 Rotation { get => Hub.PlayerCameraReference.forward; set => Hub.PlayerCameraReference.forward = value; }

        /// <summary>
        /// Gets or sets the player's scale.
        /// </summary>
        public Vector3 Scale
        {
            get => Hub.transform.localScale;
            set
            {
                Hub.transform.localScale = value;

                foreach (var player in PlayersList.List)
                    NetworkServer.SendSpawnMessage(Identitity, player.Connection);
            }
        }

        /// <summary>
        /// Gets or sets this player's current rotation.
        /// </summary>
        public Vector2 Rotations { get => Hub.playerMovementSync.NetworkRotationSync; set => Hub.playerMovementSync.NetworkRotationSync = value; }

        /// <summary>
        /// Gets or sets the player's rotation as a <see cref="Quaternion"/>.
        /// </summary>
        public Quaternion RotationsQ { get => PlayerCamera.rotation; set => PlayerCamera.transform.rotation = value; }

        /// <summary>
        /// Gets the color of the player's role.
        /// </summary>
        public Color RoleColor { get => Role.GetColor(); }

        /// <summary>
        /// Gets or sets the player's UserId.
        /// </summary>
        public string UserId { get => Hub.characterClassManager.UserId; set => Hub.characterClassManager.UserId = value; }

        /// <summary>
        /// Gets the player's raw user ID (without the identificator).
        /// </summary>
        public string RawUserId { get => Hub.characterClassManager.UserId.Split('@')[0]; }

        /// <summary>
        /// Gets the player's authentification token.
        /// </summary>
        public string AuthToken { get => Hub.characterClassManager.AuthToken; }

        /// <summary>
        /// Gets or sets the player's custom UserId.
        /// </summary>
        public string CustomUserId { get => Hub.characterClassManager.UserId2; set => Hub.characterClassManager.UserId2 = value; }

        /// <summary>
        /// Gets or sets the player's custom info string.
        /// </summary>
        public string CustomInfo { get => Hub.nicknameSync.Network_customPlayerInfoString; set => Hub.nicknameSync.Network_customPlayerInfoString = value; }

        /// <summary>
        /// Gets or sets the player's current speaker. Only applies if the player is SCP-079.
        /// </summary>
        public string CurrentSpeaker { get => Hub.scp079PlayerScript?.NetworkcurSpeaker; set => Hub.scp079PlayerScript.NetworkcurSpeaker = value; }

        /// <summary>
        /// Gets or sets the player's current unit name.
        /// </summary>
        public string UnitName { get => Hub.characterClassManager.NetworkCurUnitName; set => Hub.characterClassManager.NetworkCurUnitName = value; }

        /// <summary>
        /// Gets or sets the player's nickname.
        /// </summary>
        public string Nickname { get => Hub.nicknameSync.Network_myNickSync; set => Hub.nicknameSync.Network_myNickSync = value; }

        /// <summary>
        /// Gets or sets the player's display nickname.
        /// </summary>
        public string DisplayNickname { get => Hub.nicknameSync.Network_displayName; set => Hub.nicknameSync.Network_displayName = value; }

        /// <summary>
        /// Gets the player's IP address.
        /// </summary>
        public string IpAddress { get => Hub.queryProcessor._ipAddress; }

        /// <summary>
        /// Gets or sets the player's current level.
        /// </summary>
        public byte Level
        {
            get => Hub.scp079PlayerScript?.Lvl ?? 0;
            set
            {
                if (Hub.scp079PlayerScript == null)
                    return;

                Hub.scp079PlayerScript.Lvl = value;
                Hub.scp079PlayerScript.TargetLevelChanged(Connection, value);
            }
        }

        /// <summary>
        /// Gets or sets the player's player ID.
        /// </summary>
        public int PlayerId { get => Hub.queryProcessor.NetworkPlayerId; set => Hub.queryProcessor.NetworkPlayerId = value; }

        /// <summary>
        /// Gets or sets an ID of the player that has cuffed this player.
        /// Will return 0 if this player is not cuffed.
        /// </summary>
        public uint CufferId { get => DisarmedPlayers.Entries.Where(x => x.DisarmedPlayer == Inventory.netId).FirstOrDefault().Disarmer; set => DisarmedPlayers.SetDisarmedStatus(Inventory, PlayersList.Get(x => x.Inventory.netId == value).FirstOrDefault()?.Inventory ?? Server.Host.Inventory); }

        /// <summary>
        /// Gets or sets the player's current camera ID.
        /// </summary>
        public ushort CameraId { get => Hub.scp079PlayerScript?.currentCamera?.cameraId ?? 0; set => Hub.scp079PlayerScript?.RpcSwitchCamera(value, false); }

        /// <summary>
        /// Gets or sets the player's current item unique.
        /// </summary>
        public ushort CurrentItemSerial { get => CurrentItem?.Serial ?? 0; set => CurrentItem = Map.Items.Where(x => x.Serial == value).FirstOrDefault(); }

        /// <summary>
        /// Gets or sets the player's custom player ID. Defaults to 0.
        /// </summary>
        public int CustomPlayerId { get; set; } = 0;

        /// <summary>
        /// Gets a value indicating the player's network latency, also known as ping.
        /// </summary>
        public int Latency { get => LiteNetLib4MirrorServer.GetPing(Connection.connectionId); }

        /// <summary>
        /// Gets or sets the player's maximum health.
        /// </summary>
        public int MaxHealth { get => Hub.playerStats.maxHP; set => Hub.playerStats.maxHP = value; }

        /// <summary>
        /// Gets or sets the player's maximum artificial health.
        /// </summary>
        public int MaxArtificialHealth { get => Hub.playerStats.NetworkMaxArtificialHealth; set => Hub.playerStats.NetworkMaxArtificialHealth = value; }

        /// <summary>
        /// Gets or sets the player's current health.
        /// </summary>
        public float Health { get => Hub.playerStats.Health; set => Hub.playerStats.Health = value; }

        /// <summary>
        /// Gets or sets the player's current artificial health.
        /// </summary>
        public ushort ArtificialHealth { get => Hub.playerStats.NetworkArtificialHealth; set => Hub.playerStats.NetworkArtificialHealth = value; }

        /// <summary>
        /// Gets or sets the player's artificial health decay.
        /// </summary>
        public float ArtificialHealthDecay { get => Hub.playerStats.NetworkArtificialHpDecay; set => Hub.playerStats.NetworkArtificialHpDecay = value; }

        /// <summary>
        /// Gets or sets the player's current experience.
        /// </summary>
        public float Experience
        {
            get => Hub.scp079PlayerScript?.Exp ?? 0f;
            set
            {
                if (Hub.scp079PlayerScript == null)
                    return;

                Hub.scp079PlayerScript.Exp = value;
                Hub.scp079PlayerScript.OnExpChange();
            }
        }

        /// <summary>
        /// Gets or sets the player's current energy.
        /// </summary>
        public float Energy { get => Hub.scp079PlayerScript?.Mana ?? 0f; set => Hub.scp079PlayerScript.Mana = value; }

        /// <summary>
        /// Gets or sets the player's maximum energy.
        /// </summary>
        public float MaxEnergy
        {
            get => Hub.scp079PlayerScript?.NetworkmaxMana ?? 0f;
            set
            {
                if (Hub.scp079PlayerScript == null)
                    return;

                Hub.scp079PlayerScript.NetworkmaxMana = value;
                Hub.scp079PlayerScript.levels[Level].maxMana = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this player sent a Do Not Track signal.
        /// </summary>
        public bool DoNotTrack { get => Hub.serverRoles.DoNotTrack; }

        /// <summary>
        /// Gets a value indicating whether or not this player has Remote Admin access.
        /// </summary>
        public bool RemoteAdmin { get => Hub.serverRoles.RemoteAdmin; }

        /// <summary>
        /// Gets a value indicating whether or not this player has global Remote Admin access.
        /// </summary>
        public bool GlobalAdmin { get => Hub.serverRoles.RaEverywhere; }

        /// <summary>
        /// Gets or sets a value indicating whether this player is invisible or not.
        /// </summary>
        public bool IsInvisible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this player is in overwatch or not.
        /// </summary>
        public bool IsInOverwatch { get => Hub.serverRoles.OverwatchEnabled; set => Hub.serverRoles.SetOverwatchStatus(value); }

        /// <summary>
        /// Gets or sets a value indicating whether or not this player is handcuffed.
        /// </summary>
        public bool IsCuffed { get => Cuffer != null; }

        /// <summary>
        /// Gets a value indicating whether the player is reloading his weapon or not.
        /// </summary>
        public bool IsReloading { get => Inventory.CurInstance is Firearm firearm && !firearm.AmmoManagerModule.Standby; }

        /// <summary>
        /// Gets a value indicating whether the player is aiming or not.
        /// </summary>
        public bool IsAiming { get => Inventory.CurInstance is Firearm firearm && firearm.AdsModule.ServerAds; }

        /// <summary>
        /// Gets a value indicating whether the player has his flashlight module enabled or not.
        /// </summary>
        public bool HasFlashlightEnabled { get => Inventory.CurInstance is Firearm firearm && firearm.Status.Flags.HasFlag(FirearmStatusFlags.FlashlightEnabled); }

        /// <summary>
        /// Gets a value indicating whether the player is sprinting or not.
        /// </summary>
        public bool IsSprinting { get => MoveState == PlayerMovementState.Sprinting; }

        /// <summary>
        /// Gets a value indicating whether the player is walking or not.
        /// </summary>
        public bool IsWalking { get => MoveState == PlayerMovementState.Walking; }

        /// <summary>
        /// Gets a value indicating whether the player is jumping or not.
        /// </summary>
        public bool IsJumping { get => Hub.fpc.isJumping; }

        /// <summary>
        /// Gets a value indicating whether the player is sneaking or not.
        /// </summary>
        public bool IsSneaking { get => MoveState == PlayerMovementState.Sneaking; }

        /// <summary>
        /// Gets a value indicating whether this player is the host player or not.
        /// </summary>
        public bool IsHost { get => Hub.isDedicatedServer; }

        /// <summary>
        /// Gets a value indicating whether the player is using the alternative voice chat (V) or not.
        /// </summary>
        public bool IsUsingAltVoiceChat { get => Radio.UsingAltVoiceChat; }

        /// <summary>
        /// Gets a value indicating whether the player is using the voice chat or not.
        /// </summary>
        public bool IsUsingVoiceChat { get => Radio.UsingVoiceChat; }

        /// <summary>
        /// Gets a value indicating whether the player is using the radio or not.
        /// </summary>
        public bool IsUsingRadio { get => Radio.UsingRadio; }

        /// <summary>
        /// Gets or sets a value indicating whether the player is using the Intercom or not.
        /// </summary>
        public bool IsUsingIntercom
        {
            get => Map.icom.SpeakerObject != null && Map.icom.SpeakerObject == Hub.gameObject;
            set
            {
                if (value)
                    Map.icom.Speaker = this;
                else
                {
                    if (Map.icom.SpeakerObject != null && Map.icom.SpeakerObject == GameObject)
                        Map.icom.SpeakerObject = null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not this player is alive.
        /// </summary>
        public bool IsAlive { get => !IsDead; }

        /// <summary>
        /// Gets a value indicating whether or not this player died.
        /// </summary>
        public bool IsDead { get => Team == Team.RIP; }

        /// <summary>
        /// Gets a value indicating whether this player is a NTF or not.
        /// </summary>
        public bool IsNtf { get => Team == Team.MTF; }

        /// <summary>
        /// Gets a value indicating whether this player is a SCP or not.
        /// </summary>
        public bool IsScp { get => Team == Team.SCP; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this player has noclip on.
        /// </summary>
        public bool IsNoClipEnabled { get => Hub.serverRoles.NoclipReady; set => Hub.serverRoles.NoclipReady = value; }

        /// <summary>
        /// Gets or sets a value indicating whether or not this player has bypass mode on.
        /// </summary>
        public bool IsBypassModeEnabled { get => Hub.serverRoles.BypassMode; set => Hub.serverRoles.BypassMode = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this player is muted or not.
        /// </summary>
        public bool IsMuted
        {
            get => Dissonance.AdministrativelyMuted;
            set
            {
                Dissonance.AdministrativelyMuted = value;

                if (value)
                    MuteHandler.IssuePersistentMute(UserId);
                else
                    MuteHandler.RevokePersistentMute(UserId);
            }
        }

        public bool IsGloballyMuted
        {
            get => Dissonance.GloballyMuted;
            set
            {
                Dissonance.GloballyMuted = value;
                IsMuted = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the player is using any kind of voice chat.
        /// </summary>
        public bool IsTransmitting { get => Dissonance.IsTransmittingOnAny(); }

        /// <summary>
        /// Gets or sets a value indicating whether this player is verified or not.
        /// </summary>
        public bool IsVerified { get => Hub.characterClassManager.NetworkIsVerified; set => Hub.characterClassManager.NetworkIsVerified = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this player is Intercom muted or not.
        /// </summary>
        public bool IsIntercomMuted { get => Hub.characterClassManager.NetworkIntercomMuted; set => Hub.characterClassManager.NetworkIntercomMuted = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this player has god mode on or not.
        /// </summary>
        public bool IsGodModeEnabled { get => Hub.characterClassManager.GodMode; set => Hub.characterClassManager.GodMode = value; }

        /// <summary>
        /// Gets a value indicating whether the player has fully connected or not.
        /// </summary>
        public bool IsConnected { get => Hub?.gameObject != null; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can use the SCP-939 alt voice chat or not.
        /// </summary>
        public bool CanUseScp939Chat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can send inputs or not.
        /// </summary>
        public bool CanSendInputs { get => Hub.fpc.NetworkforceStopInputs; set => Hub.fpc.NetworkforceStopInputs = value; }

        /// <summary>
        /// Gets a value indicating whether the player has changed appearance or not.
        /// </summary>
        public bool HasChangedAppearance { get => appearance.HasValue; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can use the Intercom or not.
        /// </summary>
        public bool IntercomAsHuman { get => Dissonance.IntercomAsHuman; set => Dissonance.IntercomAsHuman = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can use SCP-939's alt voice chat or not.
        /// </summary>
        public bool MimicAs939 { get => Dissonance.MimicAs939; set => Dissonance.MimicAs939 = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can use SCP-079's speakers.
        /// </summary>
        public bool SpeakerAs079 { get => Dissonance.SpeakerAs079; set => Dissonance.SpeakerAs079 = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can use the Radio or not.
        /// </summary>
        public bool RadioAsHuman { get => Dissonance.RadioAsHuman; set => Dissonance.RadioAsHuman = value; }

        /// <summary>
        /// Gets ot sets a value indicating whether the player can use the SCP chat or not.
        /// </summary>
        public bool ScpChat { get => Dissonance.SCPChat; set => Dissonance.SCPChat = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the player can use the Spectator chat or not.
        /// </summary>
        public bool SpectatorChat { get => Dissonance.SpectatorChat; set => Dissonance.SpectatorChat = value; }

        /// <summary>
        /// Gets the distance from the other position.
        /// </summary>
        /// <param name="otherPos">The other position.</param>
        /// <returns>The distance from the other position.</returns>
        public float Distance(Vector3 otherPos) => Vector3.Distance(Position, otherPos);

        /// <summary>
        /// Resets the player's Dissonance component.
        /// </summary>
        public void ResetDissonance()
            => Dissonance.ResetToDefault();

        /// <summary>
        /// Sets the player's <see cref="RoleType"/>.
        /// </summary>
        /// <param name="newRole">The role to set.</param>
        /// <param name="keepPosition">Indicates whether to save position and inventory or not.</param>
        /// <param name="escape">Indicates whether the player has escaped or not.</param>
        public void ChangeRole(RoleType newRole, bool keepPosition = true, CharacterClassManager.SpawnReason reason = CharacterClassManager.SpawnReason.ForceClass) 
            => Hub.characterClassManager.SetClassIDAdv(newRole, keepPosition, reason);

        /// <summary>
        /// Broadcasts a message.
        /// </summary>
        /// <param name="message">The message to broadcast.</param>
        public void Broadcast(Message message) 
            => message.Show(this);

        /// <summary>
        /// Broadcasts a message.
        /// </summary>
        /// <param name="text">The text of this message.</param>
        /// <param name="duration">The duration of this message, defaults to 10 seconds.</param>
        /// <param name="type">The type of this message, defaults to <see cref="MessageType.Broadcast"/>.</param>
        /// <param name="color">The color of this message, defaults to red. Applies only for <see cref="MessageType.PlayerReport"/> and <see cref="MessageType.Console"/>.</param>
        public void Message(object text, ushort duration = 10, MessageType type = MessageType.Broadcast, string color = "red") => Broadcast(new Message
        {
            Color = color,
            Duration = duration,
            Text = text.ToString(),
            Type = type
        });

        /// <summary>
        /// Counts how many items of a certain <see cref="ItemType"/> a player has.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>How many items of that <see cref="ItemType"/> the player has.</returns>
        public int CountItem(ItemType item) => Items.Count(inventoryItem => inventoryItem.Id == item);

        /// <summary>
        /// Removes the held item from the player's inventory.
        /// </summary>
        public void RemoveItem() 
            => Hub.inventory.ServerRemoveItem(CurrentItem?.Serial ?? 0, CurrentItem?.Base.PickupDropModel);

        /// <summary>
        /// Drops an item from the player's inventory.
        /// </summary>
        /// <param name="item">The item to be dropped.</param>
        public void DropItem(BaseItem item)
            => Inventory.ServerDropItem(item?.Serial ?? 0);

        /// <summary>
        /// Indicates whether or not the player has an item.
        /// </summary>
        /// <param name="item">The item to search for.</param>
        /// <returns>true, if the player has it; otherwise, false.</returns>
        public bool HasItem(ItemType item)
        {
            foreach (BaseItem itemInfo in Items)
            {
                if (itemInfo.Id == item)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Sends a console message to the player's console.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="color">The message color.</param>
        public void SendConsoleMessage(string message, string color) => SendConsoleMessage(this, message, color);

        /// <summary>
        /// Sends a console message to a <see cref="Player"/>.
        /// </summary>
        /// <param name="target">The message target.</param>
        /// <param name="message">The message to be sent.</param>
        /// <param name="color">The message color.</param>
        public void SendConsoleMessage(Player target, string message, string color) => Hub.characterClassManager.TargetConsolePrint(target.Connection, message, color);

        /// <summary>
        /// Disconnects a <see cref="ReferenceHub">player</see>.
        /// </summary>
        /// <param name="reason">The disconnection reason.</param>
        public void Disconnect(string reason = null) => ServerConsole.Disconnect(GameObject, string.IsNullOrEmpty(reason) ? string.Empty : reason);

        /// <summary>
        /// Damages the player.
        /// </summary>
        /// <param name="damage">The damage to be inflicted.</param>
        /// <param name="damageType">The damage type.</param>
        /// <param name="attackerName">The attacker name.</param>
        /// <param name="attackerId">The attacker player id.</param>
        public void Damage(float damage, DamageTypes.DamageType damageType = default, string attackerName = "WORLD", int attackerId = 0) =>
            Hub.playerStats.HurtPlayer(new PlayerStats.HitInfo(damage, "Server", damageType, attackerId, true), GameObject);

        /// <summary>
        /// Damages the player.
        /// </summary>
        /// <param name="damage">The damage to be inflicted.</param>
        /// <param name="attacker">The attacker.</param>
        /// <param name="damageType">The damage type.</param>
        public void Damage(float damage, Player attacker, DamageTypes.DamageType damageType = default) => Damage(damage, damageType, attacker?.Nickname, attacker?.PlayerId ?? 0);

        /// <summary>
        /// Kills the player.
        /// </summary>
        /// <param name="damageType">The <see cref="DamageTypes.DamageType"/> that will kill the player.</param>
        public void Kill(DamageTypes.DamageType damageType = default) => Damage(-1f, damageType);

        /// <summary>
        /// Bans the player.
        /// </summary>
        /// <param name="duration">The ban duration.</param>
        /// <param name="reason">The ban reason.</param>
        /// <param name="issuer">The ban issuer nickname.</param>
        public void Ban(int duration, string reason, string issuer = "Console") => UnityEngine.Object.FindObjectOfType<BanPlayer>().BanUser(GameObject, duration, reason, issuer, false);

        /// <summary>
        /// Kicks the player.
        /// </summary>
        /// <param name="reason">The kick reason.</param>
        /// <param name="issuer">The kick issuer nickname.</param>
        public void Kick(string reason, string issuer = "Console") => Ban(0, reason, issuer);

        /// <summary>
        /// Sends a message to the player's Remote Admin console.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="success">Indicates whether the message should be highlighted as success or not.</param>
        /// <param name="tag">The plugin name.</param>
        public void RaMessage(string message, bool success = true, string tag = null) 
            => Sender.RaReply((tag ?? "SERVER") + "#" + message, success, true, string.Empty);

        /// <summary>
        /// A simple broadcast to a <see cref="ReferenceHub"/>. Doesn't get logged to the console and can be monospaced.
        /// </summary>
        /// <param name="duration">The broadcast duration.</param>
        /// <param name="message">The message to be broadcasted.</param>
        /// <param name="type">The broadcast type.</param>
        public void Broadcast(ushort duration, string message, global::Broadcast.BroadcastFlags type = global::Broadcast.BroadcastFlags.Normal)
            => Broadcaster.TargetAddElement(Connection, message, duration, type);

        /// <summary>
        /// Clears the player's brodcast. Doesn't get logged to the console.
        /// </summary>
        public void ClearBroadcasts() 
            => Broadcaster.TargetClearElements(Connection);

        /// <summary>
        /// Resets the player's inventory to the provided list of items, clearing any items it already possess.
        /// </summary>
        /// <param name="newItems">The new items that have to be added to the inventory.</param>
        public void ResetInventory(IEnumerable<ItemType> newItems)
        {
            ClearInventory();

            if (newItems.Count() > 0)
            {
                foreach (ItemType item in newItems)
                    AddItem(item);
            }
        }

        /// <summary>
        /// Clears the player's inventory, including all ammo and items.
        /// </summary>
        public void ClearInventory()
            => items.ForEach(x => Inventory.ServerRemoveItem(x.Serial, x.Base.PickupDropModel));

        /// <summary>
        /// Sends all items to the player's client.
        /// </summary>
        public void SendItems()
        {
            Hub.inventory.ServerSendAmmo();
            Hub.inventory.ServerSendItems();
        }

        /// <summary>
        /// Drops every item in the player's inventory (except ammo).
        /// </summary>
        public void DropItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];

                if (item.Id.IsAmmo())
                    continue;

                Inventory.ServerDropItem(item.Serial);

                items.RemoveAt(i);

                continue;
            }
        }

        /// <summary>
        /// Drops all ammo from the player's inventory.
        /// </summary>
        public void DropAmmo()
            => Ammo.DropAmmo();

        /// <summary>
        /// Drops all items from the player's inventory.
        /// </summary>
        public void DropAllItems()
            => Inventory.ServerDropEverything();

        /// <summary>
        /// Add an item of the specified type with default durability(ammo/charge) and no mods to the player's inventory.
        /// </summary>
        /// <param name="itemType">The item to be added.</param>
        /// <returns>The <see cref="ItemBase"/> given to the player.</returns>
        public BaseItem AddItem(ItemType itemType)
        {
            BaseItem item = BaseItem.Get(Inventory.ServerAddItem(itemType));

            if (item is FirearmItem firearm)
            {
                var fBase = firearm.AsBase<Firearm>();

                if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(Hub, out Dictionary<ItemType, uint> dict) &&
                    dict.TryGetValue(itemType, out uint code))
                {
                    fBase.ApplyAttachmentsCode(code, true);
                }

                FirearmStatusFlags flags = FirearmStatusFlags.MagazineInserted;

                if (fBase.CombinedAttachments.AdditionalPros.HasFlagFast(AttachmentDescriptiveAdvantages.Flashlight))
                    flags |= FirearmStatusFlags.FlashlightEnabled;

                firearm.Status = new FirearmStatus(firearm.MaxAmmo, flags, fBase.GetCurrentAttachmentsCode());
            }

            return item;
        }

        /// <summary>
        /// Returns a list of items that match the generic parameter type.
        /// </summary>
        /// <typeparam name="TItem">The type of items to retrieve.</typeparam>
        /// <returns>The list of retrieved items.</returns>
        public List<TItem> GetItems<TItem>() where TItem : ItemBase
            => Inventory.UserInventory.Items.Values.Where(x => x is TItem).Select(x => x as TItem).ToList();

        /// <summary>
        /// Add an item to the player's inventory.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        public void AddItem(BaseItem item) => AddItem(item.Base);

        /// <summary>
        /// Adds an item to the player's inventory.
        /// </summary>
        /// <param name="pickup">The <see cref="BasePickup"/> of the item to be added.</param>
        /// <returns>The <see cref="Item"/> that was added.</returns>
        public BaseItem AddItem(BasePickup pickup) => BaseItem.Get(Inventory.ServerAddItem(pickup.Id, pickup.Serial, pickup.Base));

        /// <summary>
        /// Add an item to the player's inventory.
        /// </summary>
        /// <param name="itemBase">The item to be added.</param>
        /// <returns>The item that was added.</returns>
        public BaseItem AddItem(ItemBase itemBase, BaseItem item = null)
        {
            try
            {
                if (item == null)
                    item = BaseItem.Get(itemBase);

                int ammo = -1;

                FirearmItem firearm = item.As<FirearmItem>();

                if (firearm != null)
                {
                    ammo = firearm.Ammo;
                }

                itemBase.Owner = Hub;

                Inventory.UserInventory.Items[item.Serial] = itemBase;

                if (itemBase.PickupDropModel != null)
                {
                    itemBase.OnAdded(itemBase.PickupDropModel);
                }

                var fBase = firearm.AsBase<Firearm>();

                if (AttachmentsServerHandler.PlayerPreferences.TryGetValue(Hub, out Dictionary<ItemType, uint> dict) &&
                    dict.TryGetValue(item.Id, out uint code))
                {
                    fBase.ApplyAttachmentsCode(code, true);
                }

                FirearmStatusFlags flags = FirearmStatusFlags.MagazineInserted;

                if (fBase.CombinedAttachments.AdditionalPros.HasFlagFast(AttachmentDescriptiveAdvantages.Flashlight))
                    flags |= FirearmStatusFlags.FlashlightEnabled;

                firearm.Status = new FirearmStatus(firearm.MaxAmmo, flags, fBase.GetCurrentAttachmentsCode());

                if (itemBase is IAcquisitionConfirmationTrigger acquisitionConfirmationTrigger)
                {
                    acquisitionConfirmationTrigger.ServerConfirmAcqusition();
                }

                items.Add(item);

                Inventory.SendItemsNextFrame = true;

                return item;
            }
            catch (Exception e)
            {
                Log.Error($"Failed to add an item to the player's inventory: {e}");
            }

            return null;
        }

        /// <summary>
        /// Add the amount of items to the player's inventory.
        /// </summary>
        /// <param name="item">The item to be added.</param>
        /// <param name="amount">The amount of items to be added.</param>
        public void AddItem(BaseItem item, int amount)
        {
            if (amount > 0)
            {
                for (int i = 0; i < amount; i++)
                    AddItem(item);
            }
        }

        /// <summary>
        /// Causes the player to throw a grenade.
        /// </summary>
        /// <param name="type">The <see cref="GrenadeType"/> to be thrown.</param>
        /// <param name="fullForce">Whether to throw with full or half force.</param>
        /// <returns>The <see cref="Throwable"/> item that was spawned.</returns>
        public ThrowableItem ThrowGrenade(GrenadeType type, bool fullForce = true)
        {
            ThrowableItem throwable;

            switch (type)
            {
                case GrenadeType.Flashbang:
                    throwable = BaseItem.Get<ThrowableItem>(ItemType.GrenadeFlash);
                    break;
                default:
                    throwable = BaseItem.Get<ThrowableItem>(type == GrenadeType.Scp018 ? ItemType.SCP018 : ItemType.GrenadeHE);
                    break;
            }

            ThrowItem(throwable, fullForce);

            return throwable;
        }

        /// <summary>
        /// Throw an item.
        /// </summary>
        /// <param name="throwable">The <see cref="Throwable"/> to be thrown.</param>
        /// <param name="fullForce">Whether to throw with full or half force.</param>
        public void ThrowItem(ThrowableItem throwable, bool fullForce = true)
        {
            throwable.Base.Owner = Hub;
            throwable.Throw(fullForce);
        }

        /// <summary>
        /// Add the list of items to the player's inventory.
        /// </summary>
        /// <param name="items">The list of items to be added.</param>
        public void AddItem(List<BaseItem> items)
        {
            if (items.Count > 0)
            {
                for (int i = 0; i < items.Count; i++)
                    AddItem(items[i]);
            }
        }

        /// <summary>
        /// Simple way to show a hint to the player.
        /// </summary>
        /// <param name="message">The message to be shown.</param>
        /// <param name="duration">The duration the text will be on screen.</param>
        public void ShowHint(string message, float duration = 3f)
        {
            HintParameter[] parameters = new HintParameter[]
            {
                new StringHintParameter(message),
            };

            HintDisplay.Show(new TextHint(message, parameters, null, duration));
        }

        /// <summary>
        /// Sends a hitmarker to the client.
        /// </summary>
        /// <param name="size">The size of the hitmarker.</param>
        public void ShowHitmarker(float size = 1f)
            => Hitmarker.SendHitmarker(Connection, size);

        /// <summary>
        /// Sets the player's mute status.
        /// </summary>
        /// <param name="type">The mute type to set.</param>
        /// <param name="status">The new mute status.</param>
        public void SetMute(MuteType type, bool status)
        {
            if (type == MuteType.Intercom)
                IsIntercomMuted = status;
            else
                IsMuted = status;
        }


        /// <summary>
        /// Gets the player's mute status.
        /// </summary>
        /// <param name="type">The mute type.</param>
        /// <returns>The mute status.</returns>
        public bool GetMute(MuteType type)
        {
            if (type == MuteType.Intercom)
                return IsIntercomMuted;

            return IsMuted;
        }

        /// <summary>
        /// Spawns a tantrum at the player.
        /// </summary>
        /// <returns>The spawned tantrum's game object.</returns>
        public GameObject SpawnTantrum()
            => Prefab.GetPrefab(PrefabType.Tantrum).Spawn(Position, Vector3.one, RotationsQ, true);

        /// <summary>
        /// Spawns a tantrum at the player.
        /// </summary>
        /// <param name="scale">The scale of the tantrum.</param>
        /// <returns>The spawned tantrum's game object.</returns>
        public GameObject SpawnTantrum(Vector3 scale)
            => Prefab.GetPrefab(PrefabType.Tantrum).Spawn(Position, scale, RotationsQ, true);

        /// <summary>
        /// Gets a <see cref="bool"/> describing whether or not the given <see cref="PlayerEffect">status effect</see> is currently enabled.
        /// </summary>
        /// <typeparam name="T">The <see cref="PlayerEffect"/> to check.</typeparam>
        /// <returns>A <see cref="bool"/> determining whether or not the player effect is active.</returns>
        public bool GetEffectActive<T>() where T : PlayerEffect
        {
            if (Hub.playerEffectsController.AllEffects.TryGetValue(typeof(T), out PlayerEffect playerEffect))
                return playerEffect.Duration > 0f;

            return false;
        }

        /// <summary>
        ///  Disables all currently active <see cref="PlayerEffect">status effects</see>.
        /// </summary>
        public void DisableAllEffects()
        {
            foreach (KeyValuePair<Type, PlayerEffect> effect in Hub.playerEffectsController.AllEffects)
            {
                if (effect.Value.Duration > 0f)
                    effect.Value.ServerChangeDuration(0f);
            }
        }

        /// <summary>
        /// Disables a specific <see cref="PlayerEffect">status effect</see> on the player.
        /// </summary>
        /// <typeparam name="T">The <see cref="PlayerEffect"/> to disable.</typeparam>
        public void DisableEffect<T>()
            where T : PlayerEffect => Hub.playerEffectsController.DisableEffect<T>();

        /// <summary>
        /// Disables a specific <see cref="EffectType">status effect</see> on the player.
        /// </summary>
        /// <param name="effect">The <see cref="EffectType"/> to disable.</param>
        public void DisableEffect(EffectType effect)
        {
            if (TryGetEffect(effect, out var playerEffect))
                playerEffect.ServerChangeDuration(0f, false);
        }

        /// <summary>
        /// Enables a <see cref="PlayerEffect">status effect</see> on the player.
        /// </summary>
        /// <typeparam name="T">The <see cref="PlayerEffect"/> to enable.</typeparam>
        /// <param name="duration">The amount of time the effect will be active for.</param>
        /// <param name="addDurationIfActive">If the effect is already active, setting to true will add this duration onto the effect.</param>
        public void EnableEffect<T>(float duration = 0f, bool addDurationIfActive = false)
            where T : PlayerEffect => Hub.playerEffectsController.EnableEffect<T>(duration, addDurationIfActive);

        /// <summary>
        /// Enables a <see cref="PlayerEffect">status effect</see> on the player.
        /// </summary>
        /// <param name="effect">The name of the <see cref="PlayerEffect"/> to enable.</param>
        /// <param name="duration">The amount of time the effect will be active for.</param>
        /// <param name="addDurationIfActive">If the effect is already active, setting to true will add this duration onto the effect.</param>
        public void EnableEffect(PlayerEffect effect, float duration = 0f, bool addDurationIfActive = false)
            => Hub.playerEffectsController.EnableEffect(effect, duration, addDurationIfActive);

        /// <summary>
        /// Enables a <see cref="PlayerEffect">status effect</see> on the player.
        /// </summary>
        /// <param name="effect">The name of the <see cref="PlayerEffect"/> to enable.</param>
        /// <param name="duration">The amount of time the effect will be active for.</param>
        /// <param name="addDurationIfActive">If the effect is already active, setting to true will add this duration onto the effect.</param>
        /// <returns>A bool indicating whether or not the effect was valid and successfully enabled.</returns>
        public bool EnableEffect(string effect, float duration = 0f, bool addDurationIfActive = false)
            => Hub.playerEffectsController.EnableByString(effect, duration, addDurationIfActive);

        /// <summary>
        /// Enables a <see cref="EffectType">status effect</see> on the player.
        /// </summary>
        /// <param name="effect">The <see cref="EffectType"/> to enable.</param>
        /// <param name="duration">The amount of time the effect will be active for.</param>
        /// <param name="addDurationIfActive">If the effect is already active, setting to true will add this duration onto the effect.</param>
        public void EnableEffect(EffectType effect, float duration = 0f, bool addDurationIfActive = false)
        {
            if (TryGetEffect(effect, out var pEffect))
                Hub.playerEffectsController.EnableEffect(pEffect, duration, addDurationIfActive);
        }

        /// <summary>
        /// Gets an instance of <see cref="PlayerEffect"/> by <see cref="EffectType"/>.
        /// </summary>
        /// <param name="effect">The <see cref="EffectType"/>.</param>
        /// <returns>The <see cref="PlayerEffect"/>.</returns>
        public PlayerEffect GetEffect(EffectType effect)
        {
            Hub.playerEffectsController.AllEffects.TryGetValue(effect.Type(), out var playerEffect);

            return playerEffect;
        }

        /// <summary>
        /// Tries to get an instance of <see cref="PlayerEffect"/> by <see cref="EffectType"/>.
        /// </summary>
        /// <param name="effect">The <see cref="EffectType"/>.</param>
        /// <param name="playerEffect">The <see cref="PlayerEffect"/>.</param>
        /// <returns>A bool indicating whether or not the <paramref name="playerEffect"/> was successfully gotten.</returns>
        public bool TryGetEffect(EffectType effect, out PlayerEffect playerEffect)
        {
            playerEffect = GetEffect(effect);

            return playerEffect != null;
        }

        /// <summary>
        /// Gets a <see cref="byte"/> indicating the intensity of the given <see cref="PlayerEffect">status effect</see>.
        /// </summary>
        /// <typeparam name="T">The <see cref="PlayerEffect"/> to check.</typeparam>
        /// <exception cref="ArgumentException">Thrown if the given type is not a valid <see cref="PlayerEffect"/>.</exception>
        /// <returns>The intensity of the effect.</returns>
        public byte GetEffectIntensity<T>()
            where T : PlayerEffect
        {
            if (Hub.playerEffectsController.AllEffects.TryGetValue(typeof(T), out PlayerEffect playerEffect))
                return playerEffect.Intensity;

            throw new ArgumentException("The given type is invalid.");
        }

        /// <summary>
        /// Changes the intensity of a <see cref="PlayerEffect">status effect</see>.
        /// </summary>
        /// <typeparam name="T">The <see cref="PlayerEffect"/> to change the intensity of.</typeparam>
        /// <param name="intensity">The intensity of the effect.</param>
        public void ChangeEffectIntensity<T>(byte intensity)
            where T : PlayerEffect => Hub.playerEffectsController.ChangeEffectIntensity<T>(intensity);

        /// <summary>
        /// Changes the intensity of a <see cref="PlayerEffect">status effect</see>.
        /// </summary>
        /// <param name="effect">The name of the <see cref="PlayerEffect"/> to enable.</param>
        /// <param name="intensity">The intensity of the effect.</param>
        /// <param name="duration">The new length of the effect. Defaults to infinite length.</param>
        public void ChangeEffectIntensity(string effect, byte intensity, float duration = 0) => Hub.playerEffectsController.ChangeByString(effect, intensity, duration);

        public override string ToString() => $"{PlayerId}: {(GlobalAdmin ? "[NW] " : " ")}{(DoNotTrack ? "[DNT] " : " ")}{(RemoteAdmin ? "[RA] " : "")} {Nickname} - {Role} - [{UserId}] ({IpAddress}) - {Latency} ms";
    }
}