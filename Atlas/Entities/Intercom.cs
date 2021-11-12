using Atlas.Abstractions;
using Atlas.Enums;

using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// Represents the in-game intercom.
    /// </summary>
    public class Intercom : MapObject
    {
        internal global::Intercom icom;

        internal Intercom(global::Intercom icom)
            => this.icom = icom;

        /// <inheritdoc/>
        public override uint NetId { get => icom.netId; }

        /// <summary>
        /// Gets the Intercom's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => icom.gameObject; }

        /// <summary>
        /// Gets the speaker's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject SpeakerObject { get => icom.Networkspeaker; set => icom.Networkspeaker = value; }

        /// <summary>
        /// Gets or sets the speaker.
        /// </summary>
        public Player Speaker { get => PlayersList.Get(icom.Networkspeaker); set => icom.Networkspeaker = value.GameObject; }

        /// <summary>
        /// Gets or sets the Intercom speaking area.
        /// </summary>
        public Transform Area { get => icom._area; set => icom._area = value; }

        /// <summary>
        /// Gets or sets the owner's <see cref="global::CharacterClassManager"/>.
        /// </summary>
        public CharacterClassManager CharacterClassManager { get => icom._ccm; set => icom._ccm = value; }

        /// <summary>
        /// Gets the Intercom's position.
        /// </summary>
        public Vector3 Position { get => icom.transform.position; }

        /// <summary>
        /// Gets the Intercom's rotation.
        /// </summary>
        public Quaternion Rotation { get => icom.transform.rotation; }

        /// <summary>
        /// Gets or sets the Intercom's state.
        /// </summary>
        public IntercomState State { get => (IntercomState)icom.Network_state; set => icom.Network_state = (global::Intercom.State)value; }

        /// <summary>
        /// Gets or sets a value indicating whether someone is using the bypass mode or not.
        /// </summary>
        public bool BypassSpeaking { get => icom.bypassSpeaking; set => icom.bypassSpeaking = value; }

        /// <summary>
        /// Gets or sets a value indicating whether someone muted is using the Intercom or not.
        /// </summary>
        public bool Muted { get => icom.Muted; set => icom.Muted = value; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow SCPs to speak or not.
        /// </summary>
        public bool AllowScpsToSpeak { get; set; }

        /// <summary>
        /// Gets a value indicating whether someone is speaking or not.
        /// </summary>
        public bool IsSpeaking { get => icom.speaking; }

        /// <summary>
        /// Gets or sets the Intercom's custom content.
        /// </summary>
        public string CustomContent { get => icom.CustomContent; set => icom.CustomContent = value; }

        /// <summary>
        /// Gets or sets the Intercom's text.
        /// </summary>
        public string Text { get => icom.Network_intercomText; set => icom.Network_intercomText = value; }

        /// <summary>
        /// Gets or sets the Intercom's time.
        /// </summary>
        public ushort Time { get => icom.NetworkIntercomTime; set => icom.NetworkIntercomTime = value; }

        /// <summary>
        /// Gets or sets the Intercom's trigger distance.
        /// </summary>
        public float TriggerDistance { get => icom.triggerDistance; set => icom.triggerDistance = value; }

        /// <summary>
        /// Gets or sets the Intercom's cooldown.
        /// </summary>
        public float Cooldown { get => icom.remainingCooldown; set => icom.remainingCooldown = value; }

        /// <summary>
        /// Gets or sets the Intercom's speech time.
        /// </summary>
        public float SpeechTime { get => icom._speechTime; set => icom._speechTime = value; }
    }
}
