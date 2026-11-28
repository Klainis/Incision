using UnityEngine;

namespace Incision
{
    public class LookAtCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private bool _ignoreY = false;
        [SerializeField] private bool _ignoreX = false;

        private float _initialYRotation;
        private float _initialXRotation;

        private void Awake()
        {
            if (_camera == null)
                _camera = Camera.main;
        }

        private void Start()
        {
            _initialXRotation = transform.rotation.x;
            _initialYRotation = transform.rotation.y;
        }

        private void LateUpdate()
        {
            if (_camera == null)
                return;

            Vector3 direction = _camera.transform.position - transform.position;
            
            if (_ignoreY)
                direction.y = _initialYRotation;
            if (_ignoreX)
                direction.x = _initialXRotation;

            if (direction.sqrMagnitude < 0.001f)
                return;

            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
