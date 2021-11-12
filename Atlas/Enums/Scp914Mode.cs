namespace Atlas.Enums
{
	/// <summary>
	/// Represents all SCP-914 config modes.
	/// </summary>
    public enum Scp914Mode
    {
		Dropped = 1,
		Inventory = 2,
		Held = 6,
		DroppedAndPlayerTeleport = 5,
		DroppedAndInventory = 3,
		DroppedAndHeld = 7
	}
}
