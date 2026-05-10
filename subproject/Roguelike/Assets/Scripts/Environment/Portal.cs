using UnityEngine;
using Roguelike.Core.Interfaces;
using Roguelike.World.Map;
using Roguelike.Actors.Player;

namespace Roguelike.Environment
{
    public class Portal : MonoBehaviour, IInteractable
    {
        private bool isActivated = false;

        public void Interact(GameObject interactor)
        {
            if (isActivated) return;
            isActivated = true;

            Debug.Log("Портал активирован!");

            // 1. Закрываем двери экрана
            LevelTransitionController.Instance.CloseDoors(() =>
            {
                // Это выполнится КОГДА двери полностью закроются

                // 2. Сброс кулдаунов
                if (interactor.TryGetComponent(out AbilityController abilities))
                {
                    abilities.ResetCooldowns();
                }

                // 3. Перемещение игрока
                interactor.transform.position = new Vector3(0, 0, -4);

                // 4. Полная перегенерация подземелья
                // Ищем генератор на сцене (лучше использовать Singleton, но пока найдем так)
                DungeonGenerator generator = FindFirstObjectByType<DungeonGenerator>();
                if (generator != null)
                {
                    generator.ClearAndRegenerate();
                }

                // Здесь игра ждет. Двери остаются закрытыми.
                // Вы откроете их вызовом LevelTransitionController.Instance.OpenDoors() 
                // из своего будущего скрипта выбора баффов.
            });
        }
    }
}