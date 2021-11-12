namespace Atlas.Enums
{
	/// <summary>
	/// Determines a room's shape.
	/// </summary>
    public enum RoomShape
    {
		/// <summary>
		/// Unknown
		/// </summary>
		Undefined,

		/// <summary>
		/// A normal room.
		/// </summary>
		EndRoom,

		/// <summary>
		/// A straight corridor.
		/// </summary>
		Straight,

		/// <summary>
		/// A curved corridor.
		/// </summary>
		Curve,

		/// <summary>
		/// A T intersection.
		/// </summary>
		TShape,

		/// <summary>
		/// A X intersection.
		/// </summary>
		XShape
	}
}
