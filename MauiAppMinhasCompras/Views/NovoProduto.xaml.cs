using MauiAppMinhasCompras.Models;

namespace MauiAppMinhasCompras.Views;

public partial class NovoProduto : ContentPage
{
    Produto produtoEdicao;

    public NovoProduto()
    {
        InitializeComponent();
    }

    // CONSTRUTOR PARA EDIÇÃO
    public NovoProduto(Produto p)
    {
        InitializeComponent();

        produtoEdicao = p;

        txt_descricao.Text = p.Descricao;
        txt_quantidade.Text = p.Quantidade.ToString();
        txt_preco.Text = p.Preco.ToString();
        txt_categoria.Text = p.Categoria;
    }

    private async void OnSalvar(object sender, EventArgs e)
    {
        try
        {
            if (produtoEdicao != null)
            {
                // EDITAR
                produtoEdicao.Descricao = txt_descricao.Text;
                produtoEdicao.Quantidade = Convert.ToDouble(txt_quantidade.Text);
                produtoEdicao.Preco = Convert.ToDouble(txt_preco.Text);
                produtoEdicao.Categoria = txt_categoria.Text;

                await App.Db.Update(produtoEdicao);
            }
            else
            {
                // NOVO
                Produto p = new Produto
                {
                    Descricao = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    Preco = Convert.ToDouble(txt_preco.Text),
                    Categoria = txt_categoria.Text
                };

                await App.Db.Insert(p);
            }

            await DisplayAlert("Sucesso", "Salvo com sucesso!", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro", ex.Message, "OK");
        }
    }
}