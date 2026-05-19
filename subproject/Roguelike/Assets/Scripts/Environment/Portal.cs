using UnityEngine;
using Roguelike.Core.Interfaces;
using Roguelike.World.Map;
using Roguelike.Actors.Player;
using System;

namespace Roguelike.Environment
{
    public class Portal : MonoBehaviour, IInteractable
    {
        private bool isActivated = false;

        public void Interact(GameObject interactor)
        {
            if (isActivated) return;
            isActivated = true;

            LevelTransitionController.Instance.CloseDoors(() =>
            {
                FindFirstObjectByType<BonusSelector>().InitBonusSelection();
                if (interactor.TryGetComponent(out AbilityController abilities))
                {
                    abilities.ResetCooldowns();
                }

                interactor.transform.position = new Vector3(0, 0, -4);

                DungeonGenerator generator = FindFirstObjectByType<DungeonGenerator>();
                if (generator != null)
                {
                    generator.ClearAndRegenerate();
                }
            });
        }
    }
}