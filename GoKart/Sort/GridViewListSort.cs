using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;

namespace GoKart
{
    public partial class MainWindow
    {
        private GridViewColumnHeader LastHeaderClicked = null;
        private ListSortDirection LastDirection;

        public void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var GridViewColumnHeader = e.OriginalSource as GridViewColumnHeader;

            if (GridViewColumnHeader != null)
            {
                if (GridViewColumnHeader.Role != GridViewColumnHeaderRole.Padding)
                {
                    ListSortDirection ListSortDirection;

                    if (GridViewColumnHeader != LastHeaderClicked)
                    {
                        LastDirection = ListSortDirection.Descending;
                        ListSortDirection = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (LastDirection == ListSortDirection.Ascending)
                        {
                            ListSortDirection = ListSortDirection.Descending;
                        }
                        else
                        {
                            ListSortDirection = ListSortDirection.Ascending;
                        }
                    }

                    var GridViewColumnBinding = GridViewColumnHeader.Column.DisplayMemberBinding as Binding;
                    var SortListBy = GridViewColumnBinding?.Path.Path ?? GridViewColumnHeader.Column.Header as string;

                    ListViewSort(sender, SortListBy, ListSortDirection);

                    LastHeaderClicked = GridViewColumnHeader;
                    LastDirection = ListSortDirection;
                }
            }
        }

        private void ListViewSort(object sender, string SortListBy, ListSortDirection ListSortDirection)
        {
            var ListView = sender as ListView;
            ICollectionView CollectionView = CollectionViewSource.GetDefaultView(ListView.ItemsSource);

            CollectionView?.SortDescriptions.Clear();
            SortDescription SortDescription = new SortDescription(SortListBy, ListSortDirection);
            CollectionView?.SortDescriptions.Add(SortDescription);
            CollectionView?.Refresh();
        }
    }
}