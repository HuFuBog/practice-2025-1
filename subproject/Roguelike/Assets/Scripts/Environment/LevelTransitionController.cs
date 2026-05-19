using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Roguelike.Environment
{
    public class LevelTransitionController : MonoBehaviour
    {
        public static LevelTransitionController Instance { get; private set; }

        [Header("UI Элементы")]
        [Tooltip("Левая черная панель")]
        public RectTransform leftDoor;
        [Tooltip("Правая черная панель")]
        public RectTransform rightDoor;

        [Header("Настройки")]
        public float transitionSpeed = 2f;

        private float leftClosedX = 0f;
        private float rightClosedX = 0f;
        private float leftOpenX;
        private float rightOpenX;


        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // Вычисляем ширину экрана для определения "открытой" позиции за экраном
            Canvas canvas = GetComponentInParent<Canvas>();
            float width = canvas.GetComponent<RectTransform>().rect.width;

            leftOpenX = -width;
            rightOpenX = width;

            leftDoor.anchoredPosition = new Vector2(leftOpenX, leftDoor.anchoredPosition.y);
            rightDoor.anchoredPosition = new Vector2(rightOpenX, rightDoor.anchoredPosition.y);
        }

        public void CloseDoors(Action onComplete = null)
        {
            StartCoroutine(AnimateDoors(leftClosedX, rightClosedX, onComplete));
        }

        public void OpenDoors(Action onComplete = null)
        {
            StartCoroutine(AnimateDoors(leftOpenX, rightOpenX, onComplete));
        }

        private IEnumerator AnimateDoors(float targetLeftX, float targetRightX, Action onComplete)
        {
            float t = 0f;
            Vector2 startLeft = leftDoor.anchoredPosition;
            Vector2 startRight = rightDoor.anchoredPosition;

            Vector2 targetLeft = new Vector2(targetLeftX, startLeft.y);
            Vector2 targetRight = new Vector2(targetRightX, startRight.y);

            while (t < 1f)
            {
                t += Time.deltaTime * transitionSpeed;
                float curve = Mathf.SmoothStep(0f, 1f, t);

                leftDoor.anchoredPosition = Vector2.Lerp(startLeft, targetLeft, curve);
                rightDoor.anchoredPosition = Vector2.Lerp(startRight, targetRight, curve);

                yield return null;
            }

            leftDoor.anchoredPosition = targetLeft;
            rightDoor.anchoredPosition = targetRight;

            onComplete?.Invoke();
        }
    }
}