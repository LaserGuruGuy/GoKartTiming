using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.ComponentModel;

namespace GoKart
{
    public partial class MainWindow
    {
        private GridViewColumnHeader LastHeaderClicked = null;
        private ListSortDirection? LastDirection = null;

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
                        ListSortDirection = ListSortDirection.Ascending;
                        LastDirection = ListSortDirection;
                    }
                    else
                    {
                        if (LastDirection == ListSortDirection.Ascending)
                        {
                            ListSortDirection = ListSortDirection.Descending;
                            LastDirection = ListSortDirection;
                        }
                        else if (LastDirection == ListSortDirection.Descending)
                        {
                            ListSortDirection = ListSortDirection.Ascending;
                            LastDirection = null;
                        }
                        else
                        {
                            ListSortDirection = ListSortDirection.Ascending;
                            LastDirection = ListSortDirection;
                        }
                    }

                    var GridViewColumnBinding = GridViewColumnHeader.Column.DisplayMemberBinding as Binding;
                    var SortListBy = GridViewColumnBinding?.Path.Path ?? GridViewColumnHeader.Column.Header as string;

                    if (LastDirection != null || LastHeaderClicked != GridViewColumnHeader)
                    {
                        ListViewSort(sender, SortListBy, ListSortDirection);
                    }
                    else
                    {
                        ListViewSort(sender);
                    }

                    LastHeaderClicked = GridViewColumnHeader;
                }
            }
        }

        private void ListViewSort(object sender, string SortListBy = null, ListSortDirection ListSortDirection = ListSortDirection.Ascending)
        {
            var ListView = sender as ListView;
            ICollectionView CollectionView = CollectionViewSource.GetDefaultView(ListView.ItemsSource);

            if (CollectionView?.SortDescriptions.Contains(new SortDescription(SortListBy, ListSortDirection)) == false)
            {
                CollectionView?.SortDescriptions.Clear();
                if (!string.IsNullOrEmpty(SortListBy))
                {
                    SortDescription SortDescription = new SortDescription(SortListBy, ListSortDirection);
                    CollectionView?.SortDescriptions.Add(SortDescription);
                }
            }
            CollectionView?.Refresh();
        }
    }
}