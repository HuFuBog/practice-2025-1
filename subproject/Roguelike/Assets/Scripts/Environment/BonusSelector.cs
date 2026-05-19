using System.Collections.Generic;
using UnityEngine;


namespace Roguelike.Environment
{
    public class BonusSelector : MonoBehaviour
    {
        [SerializeField] private GameObject bonusList;

        public void InitBonusSelection()
        {
            bonusList.SetActive(true);
        }
    }
}