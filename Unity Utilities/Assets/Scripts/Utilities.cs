using UnityEngine;

namespace Sonic_Prototype_Utilities
{
    public class Utilities
    {
        /// <summary>
        /// Returns true if the thumbstick is pulled or if the arrow keys or WASD keys are pressed.
        /// </summary>
        public static bool LeftThumbstickAxisIsInUse()
        {
            return Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;
        }

        public static bool RightThumbstickAxisIsInUse ()
        {
            return Input.GetAxis("Right Stick X Axis") != 0 || Input.GetAxis("Right Stick Y Axis") != 0;
        }

        public static bool TriggersAreInUse ()
        {
            return (Input.GetAxis("Left Trigger") != 0 || (Input.GetAxis("Right Trigger") != 0));
        }

        /// <summary>
        /// The amount of force currently applied to the thumbstick. 
        /// </summary>
        public static float AnalogMagnitude()
        {
            if (LeftThumbstickAxisIsInUse())
            {
                return Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")), 1).magnitude;
            }
            return 1;
        }

        /// <summary>
        /// The vector direction that the analog controls are being held in, normalized. 
        /// </summary>
        public static Vector3 AnalogDirectionVector()
        {
            return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        }

        /// <summary>
        /// The direction that the analog controls are being held in around the direction 'upwards', as a Quaternion.
        /// </summary>
        public static Quaternion AnalogDirectionQuaternion(Vector3 upwards)
        {
            if (LeftThumbstickAxisIsInUse())
            {
                return Quaternion.LookRotation(AnalogDirectionVector(), upwards);
            }
            return Quaternion.identity;
        }

        /// <summary>
        /// The direction that the analog controls are currently held in, in euler angles around the direction 'upwards'.
        /// </summary>
        public static float AnalogDirectionEulerY(Vector3 upwards)
        {
            return AnalogDirectionQuaternion(upwards).eulerAngles.y;
        }

        /// <summary>
        /// Plays an audio clip and destroys the object's renderer and collider, 
        /// and then destroys the object itself when the audio clip has ended. 
        /// </summary>
        public static void DestroyObjectWithSoundEffect (AudioSource _audioSource, AudioClip _audioClip, GameObject gameObject)
        {
            _audioSource.PlayOneShot(_audioClip);
            Object.Destroy(gameObject.GetComponent<Renderer>());
            gameObject.GetComponent<Collider>().enabled = false;
            Object.Destroy(gameObject, _audioClip.length);
        }        
    }
}