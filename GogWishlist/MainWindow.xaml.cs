using GogWishlist.classes.controller;
using System;
using System.Windows;
using System.Windows.Controls;

namespace GogWishlist
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            bConfirm.Click += bConfirm_Click;
            cbFilter.SelectionChanged += cbFilter_SelectionChanged;
            lvItems.SelectionChanged += lvItems_SelectionChanged;
            cbDiscount.Checked += cbDiscount_Checked;
            cbDiscount.Unchecked += cbDiscount_Checked;
        }

        void cbDiscount_Checked(object sender, RoutedEventArgs e)
        {
            ControllerData.Instance.FilterDiscounted = cbDiscount.IsChecked == null ? false : cbDiscount.IsChecked.Value;
            lvItems.ItemsSource = ControllerData.Instance.Data;
        }

        void lvItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string url = ControllerData.Instance.ItemPageUrl(lvItems.SelectedIndex);

            if (String.IsNullOrEmpty(url))
            {
                MessageBox.Show(Messages.error_page_address);
                return;
            }

            try
            {
                System.Diagnostics.Process.Start(url);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }

        void cbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ControllerData.Instance.SortOrder = cbFilter.SelectedIndex == 0 ? ControllerData.SortOrderEnum.price : ControllerData.SortOrderEnum.discount;
            lvItems.ItemsSource = ControllerData.Instance.Data;
        }

        private void bConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbUsername.Text))
            {
                MessageBox.Show(Messages.error_username);
                return;
            }

            bConfirm.IsEnabled = false;
            cbFilter.IsEnabled = false;
            cbDiscount.IsEnabled = false;
            ControllerData.Instance.Username = tbUsername.Text;
            downloadData();
        }

        private async void downloadData()
        {

            UpdateResponse responseType = await ControllerData.Instance.updateData();

            switch (responseType.Type)
            {
                case UpdateResponse.ResponseType.type_ok:
                    lvItems.ItemsSource = ControllerData.Instance.Data;
                    break;
                case UpdateResponse.ResponseType.type_error_response_cache_not_found:
                    MessageBox.Show(Messages.error_response_cache_not_found);
                    break;
                case UpdateResponse.ResponseType.type_error_response_empty:

                    MessageBox.Show(Messages.error_response_empty);
                    break;
                case UpdateResponse.ResponseType.type_error_web:
                    MessageBox.Show(responseType.Message);
                    break;
                case UpdateResponse.ResponseType.type_error_wishlist_data:
                    MessageBox.Show(Messages.error_wishlist_data);
                    break;
                case UpdateResponse.ResponseType.type_error_wishlist_products_not_found:
                    MessageBox.Show(Messages.error_wishlist_products_not_found);
                    break;
            }

            bConfirm.IsEnabled = true;
            cbFilter.IsEnabled = true;
            cbDiscount.IsEnabled = true;
        }
    }
}
