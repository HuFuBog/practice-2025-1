using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Roguelike.UI
{
    public class LineCollection : MonoBehaviour
    {
        [SerializeField] private List<GameObject> variants;
        [SerializeField] private TMP_Text description;
        public List<string> values;
        public List<string> descriptions;
        public int choosedId { get; private set; }
        public string choosedValue { get; private set; }

        public void SetControlled(int id)
        {
            for (int i = 0; i < variants.Count; ++i)
            {
                variants[i].transform.GetChild(0).gameObject.SetActive(i == id);
            }
            choosedId = id;
            description.text = descriptions[choosedId];
            choosedValue = values[choosedId];
            description.gameObject.SetActive(true);
        }
    }
}

