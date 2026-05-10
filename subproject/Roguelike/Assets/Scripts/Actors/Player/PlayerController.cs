using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Подключаем новую систему ввода
using Roguelike.Core.Interfaces;
namespace Roguelike.Actors.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(AbilityController))]
    [RequireComponent(typeof(Combat.HealthSystem))]
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Dash Settings")]
        public float dashMultiplier = 3f;
        public float dashDuration = 0.2f;
        public float dashCooldown = 1f;

        private Rigidbody2D rb;
        private AbilityController abilities;
        private Combat.HealthSystem healthSystem;
        private PlayerInput playerInput;

        // это добро в кеш, чтобы не спамить запросами в будущем
        private InputAction moveAction;
        private InputAction dashAction;
        private InputAction attackAction;
        private InputAction skillAction;
        private InputAction ultimateAction;

        private Vector2 moveInput;
        private Vector2 lastLookDir = Vector2.right;

        private bool isDashing;
        private float dashTimer;
        private int originalLayer;

        private InputAction interactAction;
        private IInteractable currentInteractable;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            abilities = GetComponent<AbilityController>();
            healthSystem = GetComponent<Combat.HealthSystem>();
            playerInput = GetComponent<PlayerInput>();


            moveAction = playerInput.actions["Move"];
            dashAction = playerInput.actions["Dash"];
            attackAction = playerInput.actions["Attack"];
            skillAction = playerInput.actions["Skill"];
            ultimateAction = playerInput.actions["Ultimate"];
            interactAction = playerInput.actions["Interact"];

            rb.gravityScale = 0f;
            originalLayer = gameObject.layer;
        }

        private void Update()
        {
            if (isDashing) return;

            dashTimer -= Time.deltaTime;

            moveInput = moveAction.ReadValue<Vector2>().normalized;

            if (moveInput != Vector2.zero) lastLookDir = moveInput;

            if (dashAction.WasPressedThisFrame() && dashTimer <= 0 && moveInput != Vector2.zero)
            {
                StartCoroutine(DashRoutine());
            }

            if (attackAction.IsPressed()) abilities.UseAttack(lastLookDir);

            if (skillAction.WasPressedThisFrame()) abilities.UseSkill(lastLookDir);
            if (ultimateAction.WasPressedThisFrame()) abilities.UseUltimate(lastLookDir);

            if (interactAction.WasPressedThisFrame() && currentInteractable != null)
            {
                currentInteractable.Interact(gameObject);
            }
        }

        private void FixedUpdate()
        {
            if (isDashing) return;

            float currentSpeed = healthSystem.baseData.baseMoveSpeed;
            rb.linearVelocity = moveInput * currentSpeed; //потом переписать на linearVelocity
        }

        private IEnumerator DashRoutine()
        {

            isDashing = true;
            dashTimer = dashCooldown;
            // на слое Invincible персонаж будет "невидим" для атак -> базовая логика рывков
            gameObject.layer = LayerMask.NameToLayer("Invincible");

            float currentSpeed = healthSystem.baseData.baseMoveSpeed;
            rb.linearVelocity = lastLookDir * (currentSpeed * dashMultiplier);

            yield return new WaitForSeconds(dashDuration);

            gameObject.layer = originalLayer;
            isDashing = false;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IInteractable interactable))
            {
                if (currentInteractable == interactable)
                {
                    currentInteractable = null;
                }
            }
        }
    }
}