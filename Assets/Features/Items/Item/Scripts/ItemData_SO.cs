using UnityEngine;

namespace Features.Items
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "Blueprints/Items/Item", order = 0)]
    public class ItemData_SO : ScriptableObject
    {
        [SerializeField] private ItemData data;

        public ItemData Data => data;
    }
}