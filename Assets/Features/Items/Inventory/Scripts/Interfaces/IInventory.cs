using System.Collections.Generic;

namespace Features.Items.Inventory
{
    public interface IInventory<T> : IList<T>
    {
        void Move(int from, int to);
    }
}