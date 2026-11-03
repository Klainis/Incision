using UnityEngine;

namespace Incision
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void LateUpdate()
        {
            if (_camera == null)
                return;

            Vector3 direction = _camera.transform.position - transform.position;

            if (direction.sqrMagnitude < 0.001f)
                return;

            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
