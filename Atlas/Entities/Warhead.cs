﻿using System;

namespace Atlas.Entities
{
    /// <summary>
    /// A set of tools to easily work with the alpha warhead.
    /// </summary>
    public static class Warhead
    {
        private static AlphaWarheadController controller;
        private static AlphaWarheadNukesitePanel sitePanel;
        private static AlphaWarheadOutsitePanel outsitePanel;

        /// <summary>
        /// Gets the cached <see cref="AlphaWarheadController"/> component.
        /// </summary>
        public static AlphaWarheadController Controller
        {
            get
            {
                if (controller == null)
                    controller = PlayerManager.localPlayer.GetComponent<AlphaWarheadController>();

                return controller;
            }
        }

        /// <summary>
        /// Gets the cached <see cref="AlphaWarheadNukesitePanel"/> component.
        /// </summary>
        public static AlphaWarheadNukesitePanel SitePanel
        {
            get
            {
                if (sitePanel == null)
                    sitePanel = UnityEngine.Object.FindObjectOfType<AlphaWarheadNukesitePanel>();

                return sitePanel;
            }
        }

        /// <summary>
        /// Gets the cached <see cref="AlphaWarheadOutsitePanel"/> component.
        /// </summary>
        public static AlphaWarheadOutsitePanel OutsitePanel
        {
            get
            {
                if (outsitePanel == null)
                    outsitePanel = UnityEngine.Object.FindObjectOfType<AlphaWarheadOutsitePanel>();

                return outsitePanel;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the warhead lever is enabled or not.
        /// </summary>
        public static bool LeverStatus
        {
            get => SitePanel.Networkenabled;
            set => SitePanel.Networkenabled = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the warhead has already been activated or not.
        /// </summary>
        public static bool IsKeycardActivated
        {
            get => OutsitePanel.NetworkkeycardEntered;
            set => OutsitePanel.NetworkkeycardEntered = value;
        }

        /// <summary>
        /// Gets a value indicating whether the warhead has already been detonated or not.
        /// </summary>
        public static bool IsDetonated => Controller.detonated;

        /// <summary>
        /// Gets a value indicating whether the warhead detonation is in progress or not.
        /// </summary>
        public static bool IsInProgress => Controller.NetworkinProgress;

        /// <summary>
        /// Gets or sets the warhead detonation timer.
        /// </summary>
        public static float DetonationTimer
        {
            get => Controller.NetworktimeToDetonation;
            set => Controller.NetworktimeToDetonation = value;
        }

        /// <summary>
        /// Gets the warhead real detonation timer.
        /// </summary>
        public static float RealDetonationTimer => Controller.RealDetonationTime();

        /// <summary>
        /// Gets or sets a value indicating whether the warhead can be disabled or not.
        /// </summary>
        public static bool IsLocked
        {
            get => Controller._isLocked;
            set => Controller._isLocked = value;
        }

        /// <summary>
        /// Gets a value indicating whether the warhead can be started or not.
        /// </summary>
        public static bool CanBeStarted => !Map.recontain._alreadyRecontained &&
            ((AlphaWarheadController._resumeScenario == -1 &&
            Math.Abs(Controller.scenarios_start[AlphaWarheadController._startScenario].SumTime() - Controller.timeToDetonation) < 0.0001f) ||
            (AlphaWarheadController._resumeScenario != -1 &&
            Math.Abs(Controller.scenarios_resume[AlphaWarheadController._resumeScenario].SumTime() - Controller.timeToDetonation) < 0.0001f));

        /// <summary>
        /// Starts the warhead countdown.
        /// </summary>
        public static void Start()
        {
            Controller.InstantPrepare();
            Controller.StartDetonation();
        }

        /// <summary>
        /// Stops the warhead.
        /// </summary>
        public static void Stop() => Controller.CancelDetonation();

        /// <summary>
        /// Detonates the warhead.
        /// </summary>
        public static void Detonate()
        {
            Controller.InstantPrepare();
            Controller.Detonate();
        }

        /// <summary>
        /// Shake all players, like if the warhead has been detonated.
        /// </summary>
        public static void Shake() => Controller.RpcShake(true);
    }
}
