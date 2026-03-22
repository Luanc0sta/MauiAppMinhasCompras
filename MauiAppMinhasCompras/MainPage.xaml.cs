using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras;

public partial class MainPage : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public MainPage()
    {
        InitializeComponent();
        ListaProdutos.ItemsSource = lista;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        lista.Clear();

        var produtos = await App.Db.GetAll();

        foreach (var p in produtos)
            lista.Add(p);
    }

    private async void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        string textoBusca = e.NewTextValue;

        lista.Clear();

        var produtos = await App.Db.Search(textoBusca);

        foreach (var p in produtos)
            lista.Add(p);
    }
}