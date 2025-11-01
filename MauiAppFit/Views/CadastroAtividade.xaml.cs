using MauiAppFit.Models;
using MauiAppFit.ViewModels;
namespace MauiAppFit.Views;

public partial class CadastroAtividade : ContentPage
{
	public CadastroAtividade()
	{
		InitializeComponent();

		BindingContext = new CadastroAtividadeViewModel();

	}

    protected override void OnAppearing()
    {
		var vm = (CadastroAtividadeViewModel)BindingContext;

		if(vm.Id == 0)
		{
			vm.NovaAtividade.Execute(null);
		}

		base.OnAppearing();
    }
}