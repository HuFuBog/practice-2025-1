using System;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelike.UI
{
    public class LineCollection : MonoBehaviour
    {
        [SerializeField] private List<GameObject> variants;
        public List<string> values;
        public int choosedId { get; private set; }
        public string choosedValue { get; private set; }

        public void SetControlled(int id)
        {
            variants[id].transform.GetChild(0).gameObject.SetActive(true);
            choosedId = id;
            choosedValue = values[choosedId];
        }
    }
}

