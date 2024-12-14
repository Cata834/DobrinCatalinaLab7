namespace DobrinCatalinaLab7;
using DobrinCatalinaLab7.Models;
using Microsoft.Maui.Controls;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
{
        var slist = (ShopList)BindingContext;
        slist.Date = DateTime.UtcNow;
        Shop selectedShop = (ShopPicker.SelectedItem as Shop);
        slist.ShopID = selectedShop.ID;
        await App.Database.SaveShopListAsync(slist);
        await Navigation.PopAsync();
    }
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        // Navigãm la ProductPage, având în vedere cã BindingContext este un obiect ShopList
        var shoplist = (ShopList)BindingContext;

        // Deschidem ProductPage ?i îi setãm BindingContext pentru a adãuga produse
        await Navigation.PushAsync(new ProductPage(shoplist));
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Verificãm dacã existã un element selectat în ListView
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
            // ?tergem produsul selectat
            await App.Database.DeleteProductAsync(selectedProduct);

            // Actualizãm lista
            var shopl = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
        }
        else
        {
            // Afi?ãm o eroare dacã nu s-a selectat nimic
            await DisplayAlert("Error", "Please select a product to delete", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        var items = await App.Database.GetShopsAsync();
        ShopPicker.ItemsSource = (System.Collections.IList)items;
        ShopPicker.ItemDisplayBinding = new Binding("ShopDetails");
        var shopl = (ShopList)BindingContext;

        listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
    }
    async void OnDeleteItemClicked(object sender, EventArgs e)
    {
        
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
            
            await App.Database.DeleteProductAsync(selectedProduct);
            
            var shopl = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
        }
        else
        {
            
            await DisplayAlert("Error", "Please select a product to delete", "OK");
        }
    }

}

