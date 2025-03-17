using MauiAppMinhasCompras.Models;
using System.Collections.ObjectModel;

namespace MauiAppMinhasCompras.Views;


public partial class ListaProduto : ContentPage
{
	ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

	private CancellationTokenSource _cancellationTokenSource;


	public ListaProduto()
	{
		InitializeComponent();

		lst_produtos.ItemsSource = lista;

	}

    protected async override void OnAppearing()
    {
		List<Produto> tmp = await App.Db.GetAll();

        lista.Clear();
        tmp.ForEach(i => lista.Add(i));
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		try
		{
			Navigation.PushAsync(new Views.NovoProduto());

		} catch (Exception ex) 
		{
			DisplayAlert("Ops", ex.Message, "OK");
		}
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
		string q = e.NewTextValue;

        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            await Task.Delay(300, _cancellationTokenSource.Token);

            if (string.IsNullOrEmpty(q))
            {
                lista.Clear();
                List<Produto> allProducts = await App.Db.GetAll();
                allProducts.ForEach(p => lista.Add(p));
            }
            else
            {
                List<Produto> filteredProducts = await App.Db.Search(q);
                lista.Clear();
                filteredProducts.ForEach(p => lista.Add(p));
            }
        }
        catch (TaskCanceledException)
        {

        }
        catch (Exception ex) 
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }

    }

    private void ToolbarItem_Clicked_1(object sender, EventArgs e)
    {
		double soma = lista.Sum(i => i.Total);

		string msg = $"O total é {soma:C}";

		DisplayAlert("Total dos Produtos", msg, "OK");
    }

    private void MenuItem_Clicked(object sender, EventArgs e)
    {

    }
}