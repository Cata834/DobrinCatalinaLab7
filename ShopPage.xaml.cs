namespace DobrinCatalinaLab7;
using DobrinCatalinaLab7.Models;
using Microsoft.Maui.Devices.Sensors;
using Plugin.LocalNotification;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();

        BindingContext = new Shop(); // sau obiectul magazinului existent
    }


    public Location location { get; private set; }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        await App.Database.SaveShopAsync(shop);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;

        // Confirm deletion with the user
        bool answer = await DisplayAlert("Confirm Deletion", "Are you sure you want to delete this shop?", "Yes", "No");

        if (answer)
        {
            // Call delete method from the database helper
            await App.Database.DeleteShopAsync(shop);
            await Navigation.PopAsync();  // Return to the previous page after deletion
        }
    }

    async void OnShowMapButtonClicked(object sender, EventArgs e)
    {
        var shop = (Shop)BindingContext;
        var address = shop.Adress;
        var locations = await Geocoding.GetLocationsAsync(address);

        var options = new MapLaunchOptions
        {
            Name = "Magazinul meu preferat"
        };

        var shoplocation = locations?.FirstOrDefault(); // ?? new Location(46.7492379, 23.5745597);

        // Simulated current location for Windows Machine
        var myLocation = new Location(46.7731796289, 23.6213886738);

        var distance = myLocation.CalculateDistance(shoplocation, DistanceUnits.Kilometers);
        if (distance < 5)
        {
            var request = new NotificationRequest
            {
                Title = "Ai de facut cumparaturi in apropiere!",
                Description = address,
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1)
                }
            };
            LocalNotificationCenter.Current.Show(request);
        }

        await Map.OpenAsync(shoplocation, options);
    }
}

  