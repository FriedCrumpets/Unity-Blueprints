using System;
using System.Collections.Generic;
using Blueprints.Command;

namespace Blueprints.Items
{
    //TODO : Is this an inventory??? T has a list of modifications. 
    /*
     * Perhaps this is an inventory of Modifiers that lives on an item
     * The modifiers are executed upon being added to the item and can modify said Item
     * ?? Removed on modifier finishing ?? this will need to be handled by the modifier itself...
     * Is that plausible ???? 
     */
    
    public interface IModifiable<T> where T : Item
    {
        int MaxNumberOfModifiers { get; set; }
        
        List<Modifier<T>> Modifiers { get; set; }

        T AddModifier(Modifier<T> modifier);
        
        T RemoveModifier(Modifier<T> modifier);
        
        bool ModifierExists(Modifier<T> modifier);

        bool MaxModifiersReached(IModifiable<T> modifiers);
    }

    public class Modifier<T> : Command<T>
    {
        public Modifier(T receiver, Action<T> execute, Action<T> undo = null) : base(receiver, execute, undo) { }
    }
}