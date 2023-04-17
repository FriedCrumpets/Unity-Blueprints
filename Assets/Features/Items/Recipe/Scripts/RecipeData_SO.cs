using UnityEngine;

namespace Features.Items.Recipe
{
    [CreateAssetMenu(fileName = "Recipe", menuName = "Blueprints/Items/Recipe", order = 0)]
    public class RecipeData_SO : ScriptableObject
    {
        [SerializeField] private RecipeData data;

        public RecipeData Data => data;
    }
}