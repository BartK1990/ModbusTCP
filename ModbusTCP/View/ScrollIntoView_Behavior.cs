using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace ModbusTCP.View
{
    /// <summary>  
    /// Represents a ListBox with events for when adding and removing items.
    /// </summary>  
    public abstract class ListBoxWithEvents : ListBox
    {
        public event EventHandler<ListBoxItemEventArgs> ItemAdded;
        public event EventHandler<ListBoxItemEventArgs> ItemRemoved;

        protected ListBoxWithEvents() : base()
        {
            ((INotifyCollectionChanged)Items).CollectionChanged += HandleCollectionChanged;
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null && e.OldItems == null)
            {
                // seems we got a new collection
                foreach (var x in Items)
                {
                    OnItemAdded(Items.IndexOf(x));
                }
            }

            if (e.NewItems != null)
            {
                foreach (var x in e.NewItems)
                {
                    OnItemAdded(Items.IndexOf(x));
                }
            }

            if (e.OldItems != null)
            {
                foreach (var x in e.OldItems)
                {
                    OnItemRemoved(Items.IndexOf(x));
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                // do smth
            }
        }

        protected virtual void OnItemAdded(int index)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, new ListBoxItemEventArgs(index));
            }
        }

        protected virtual void OnItemRemoved(int index)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, new ListBoxItemEventArgs(index));
            }
        }
    }

    public class ListBoxItemEventArgs : EventArgs
    {
        public ListBoxItemEventArgs(int index)
        {
            Index = index;
        }
        public int Index { get; private set; }
    }

    public class ListBoxScroll : ListBoxWithEvents
    {
        protected override void OnItemAdded(int index)
        {
            base.OnItemAdded(index);
            ScrollIntoView(Items[index]);
        }
    } 
}
