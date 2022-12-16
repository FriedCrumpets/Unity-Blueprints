using System.Collections.Generic;
using Blueprints.Inventory;

namespace Blueprints.Crafting
{
    public interface IModdable
    {
        void AddModifier();
        void RemoveModifier();
    }
    
    public interface IModdable<T> where T : Item
    {
        List<Modifier<T>> Modifiers { get; set; }
    }

    public class Modifier<T> where T : IModdable
    {
        
    }
}