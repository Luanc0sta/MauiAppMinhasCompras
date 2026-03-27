using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;

public partial class ListaProduto : ContentPage
{
    ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

    public ListaProduto()
    {
        InitializeComponent();
        lista_produtos.ItemsSource = lista;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        try
        {
            lista.Clear();

            var produtos = await App.Db.GetAll();

            produtos.ForEach(p => lista.Add(p));
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

    // EDITAR AO CLICAR
    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Produto p = e.SelectedItem as Produto;

            if (p != null)
            {
                await Navigation.PushAsync(new NovoProduto(p));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // EXCLUIR
    private async void OnDelete(object sender, EventArgs e)
    {
        try
        {
            MenuItem item = sender as MenuItem;
            Produto produto = item.BindingContext as Produto;

            bool confirm = await DisplayAlert("Excluir", "Deseja excluir?", "Sim", "Não");

            if (confirm)
            {
                await App.Db.Delete(produto.Id);
                lista.Remove(produto);
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // FILTRO POR CATEGORIA (CORRIGIDO)
    private async void OnCategoriaSelecionada(object sender, EventArgs e)
    {
        try
        {
            if (pickerCategoria.SelectedIndex == -1)
                return;

            string categoria = pickerCategoria.SelectedItem.ToString();

            lista.Clear();

            var produtos = await App.Db.GetAll();

            if (categoria == "Todos")
            {
                produtos.ForEach(p => lista.Add(p));
            }
            else
            {
                var filtrados = produtos
                    .Where(p => p.Categoria != null &&
                                p.Categoria.ToLower() == categoria.ToLower())
                    .ToList();

                filtrados.ForEach(p => lista.Add(p));
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }

    // RELATÓRIO POR CATEGORIA
    private async void OnRelatorioCategoria(object sender, EventArgs e)
    {
        try
        {
            var produtos = await App.Db.GetAll();

            var relatorio = produtos
                .GroupBy(p => p.Categoria)
                .Select(g => new
                {
                    Categoria = g.Key,
                    Total = g.Sum(p => p.Total)
                });

            string msg = "";

            foreach (var item in relatorio)
            {
                msg += $"{item.Categoria}: {item.Total:C}\n";
            }

            await DisplayAlert("Relatório por Categoria", msg, "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}