using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    public ListaProduto()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        try
        {
            lista_produtos.ItemsSource = await App.Db.GetAll();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // BOTÃO +
    private async void OnAdd(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NovoProduto());
    }

    // EXCLUIR
    private async void OnDelete(object sender, EventArgs e)
    {
        try
        {
            MenuItem item = sender as MenuItem;
            Produto produto = item.BindingContext as Produto;

            bool resposta = await DisplayAlert(
                "Confirmar",
                "Deseja excluir?",
                "Sim",
                "Não"
            );

            if (resposta)
            {
                await App.Db.Delete(produto.Id);
                lista_produtos.ItemsSource = await App.Db.GetAll();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // EDITAR
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto produto = e.SelectedItem as Produto;

            if (produto != null)
            {
                await Navigation.PushAsync(new EditarProduto
                {
                    BindingContext = produto
                });
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}