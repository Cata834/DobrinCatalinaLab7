namespace DobrinCatalinaLab7;
using DobrinCatalinaLab7.Models;

public partial class ListPage : ContentPage
{
	public ListPage()
	{
		InitializeComponent();
	}
    async void OnSaveButtonClicked(object sender, EventArgs e)
{
    var shoplist = (ShopList)BindingContext;
    if (!string.IsNullOrWhiteSpace(shoplist.Description))
    {
        await App.Database.SaveShopListAsync(shoplist);
        await Navigation.PopAsync();
    }
    else
    {
        await DisplayAlert("Error", "Please enter a description.", "OK");
    }
}
    async void OnChooseButtonClicked(object sender, EventArgs e)
    {
        // Navig�m la ProductPage, av�nd �n vedere c� BindingContext este un obiect ShopList
        var shoplist = (ShopList)BindingContext;

        // Deschidem ProductPage ?i �i set�m BindingContext pentru a ad�uga produse
        await Navigation.PushAsync(new ProductPage(shoplist));
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        // Verific�m dac� exist� un element selectat �n ListView
        var selectedProduct = listView.SelectedItem as Product;

        if (selectedProduct != null)
        {
            // ?tergem produsul selectat
            await App.Database.DeleteProductAsync(selectedProduct);

            // Actualiz�m lista
            var shopl = (ShopList)BindingContext;
            listView.ItemsSource = await App.Database.GetListProductsAsync(shopl.ID);
        }
        else
        {
            // Afi?�m o eroare dac� nu s-a selectat nimic
            await DisplayAlert("Error", "Please select a product to delete", "OK");
        }
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
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

