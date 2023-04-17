using UnityEngine;

namespace Features.Items.Recipe
{
    public static class Recipes
    {
        public static GameObject Craft(RecipeData data)
            => Object.Instantiate(data.ItemToCraft.GameObject);
    }
}