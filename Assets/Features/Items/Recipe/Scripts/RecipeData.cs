using System.Collections.Generic;
using UnityEngine;

namespace Features.Items.Recipe
{
    [System.Serializable]
    public struct RecipeData
    {
        [SerializeField] private ItemData itemToCraft;
        [SerializeField] private List<ItemData> recipe;

        public ItemData ItemToCraft => itemToCraft;
        public List<ItemData> Recipe => recipe;
    }
}