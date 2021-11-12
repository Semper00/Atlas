namespace Atlas.Enums
{
	/// <summary>
	/// Represents in-game target button types.
	/// </summary>
    public enum TargetButtonType
    {
		/// <summary>
		/// Sets the target's maximum health to it's maximum health incremented by two.
		/// </summary>
		IncreaseHealth,

		/// <summary>
		/// Sets the target's maximum health to it's maximum health divided by two.
		/// </summary>
		DecreaseHealth,

		/// <summary>
		/// Increases the target's reset time by one.
		/// </summary>
		IncreaseResetTime,

		/// <summary>
		/// Decreases the target's reset time by one.
		/// </summary>
		DecreaseResetTime,

		/// <summary>
		/// Resets the target.
		/// </summary>
		ManualReset,

		/// <summary>
		/// Deletes the target (this does nothing in the base game).
		/// </summary>
		Remove,

		/// <summary>
		/// Shows global results - does nothing yet.
		/// </summary>
		GlobalResults
	}
}
