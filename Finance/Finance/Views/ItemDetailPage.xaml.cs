using Finance.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Finance.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}