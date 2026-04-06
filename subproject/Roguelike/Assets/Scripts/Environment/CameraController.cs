using UnityEngine;

namespace Roguelike.Environment
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float speed = 5;
        [SerializeField] private float distanceToUntouch = 5;
        [SerializeField] private bool isLocked;
        [SerializeField] private Transform defaultTarget;
        [SerializeField] private Transform target;
        public Vector3 offset;
        void Awake()
        {
            if (!target)
            {
                target = defaultTarget;
            }
        }
        public void Lock(Transform target)
        {
            if (isLocked) return;
            isLocked = true;
            this.target = target;
        }
        public void Unlock()
        {
            isLocked = false;
            target = defaultTarget;
        }
        public void Update()
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
        }
    }
}