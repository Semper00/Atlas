using UnityEngine;

namespace Atlas.Entities
{
    /// <summary>
    /// A wrapper around the <see cref="Camera079"/> class for easier camera management.
    /// </summary>
    public class Camera
    {
        internal Camera079 cam;

        /// <summary>
        /// Initializes a new instance of the <see cref="Camera"/> class.
        /// </summary>
        /// <param name="cam">The original camera.</param>
        internal Camera(Camera079 cam, bool addToApi = false)
        {
            this.cam = cam;

            Room = Room.Get(cam.gameObject);

            Type = (Enums.CameraType)cam.GetInstanceID();

            if (addToApi)
                Map.cams.Add(this);
        }

        /// <summary>
        /// Gets the camera's <see cref="UnityEngine.GameObject"/>.
        /// </summary>
        public GameObject GameObject { get => cam.gameObject; }

        /// <summary>
        /// Gets the room this camera is located in.
        /// </summary>
        public Room Room { get; }

        /// <summary>
        /// Gets the camera's type.
        /// </summary>
        public Enums.CameraType Type { get; }

        /// <summary>
        /// Gets the camera's network ID.
        /// </summary>
        public int NetId { get => cam.GetInstanceID(); }

        /// <summary>
        /// Gets the camera's position.
        /// </summary>
        public Vector3 Position { get => GameObject.transform.position; }

        /// <summary>
        /// Gets or sets (not recommended) the camera ID.
        /// </summary>
        public ushort Id { get => cam.cameraId; set => cam.cameraId = value; }

        /// <summary>
        /// Gets or sets the camera name.
        /// </summary>
        public string Name { get => cam.cameraName; set => cam.cameraName = value; }

        /// <summary>
        /// Gets or sets the camera pitch.
        /// </summary>
        public float Pitch { get => cam.curPitch; set => cam.curPitch = value; }

        /// <summary>
        /// Gets or sets the camera rotation.
        /// </summary>
        public float Rotation { get => cam.curRot; set => cam.curRot = value; }

        /// <summary>
        /// Updates the camera's position.
        /// </summary>
        public void UpdatePosition() => cam.UpdatePosition(Rotation, Pitch);

        /// <summary>
        /// Gets a <see cref="Camera"/> from a <see cref="Camera079"/>. This method will ALWAYS return an instance as it creates a new one in case it wasn't found.
        /// </summary>
        /// <param name="camera">The <see cref="Camera079"/> to find.</param>
        /// <returns>The <see cref="Camera"/> instance.</returns>
        public static Camera Get(Camera079 camera)
        {
            foreach (Camera cam in Map.cams)
            {
                if (cam.NetId == camera.GetInstanceID())
                    return cam;
            }

            return new Camera(camera); 
        }

        /// <summary>
        /// Gets a <see cref="Camera"/> from a <see cref="Enums.CameraType"/>. Will return null if it fails to find that type.
        /// </summary>
        /// <param name="type">The <see cref="Enums.CameraType"/> to find.</param>
        /// <returns>The <see cref="Camera"/> instance.</returns>
        public static Camera Get(Enums.CameraType type)
        {
            foreach (Camera cam in Map.cams)
            {
                if (cam.Type == type)
                    return cam;
            }

            return null;
        }
    }
}