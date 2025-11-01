using MauiAppFit.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiAppFit.ViewModels
{
   
    public class ListaAtividadesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        //pegando o texto qye foi digitado pelo usuario

        public string ParametroBusca {  get; set; }

        // gerencia se mostra ao usuario o refreshView

        bool estaAtualizando = false;

        public bool EstaAtualizando
        {
            get => estaAtualizando;
            set
            {
                estaAtualizando = value;
                PropertyChanged(this, new PropertyChangedEventArgs("EstaAtualizando"));
            }
        }

        //Armazena as atividades do usuario

        ObservableCollection<Atividade> listaAtividades = new ObservableCollection<Atividade>();

        public ObservableCollection<Atividade> ListaAtividades
        {
            get => listaAtividades;
            set => listaAtividades = value;
        }

        public ICommand Atualizarlista
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;
                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.GetAllRows();

                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }

        public ICommand Buscar
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (EstaAtualizando)
                            return;
                        EstaAtualizando = true;

                        List<Atividade> tmp = await App.Database.Search(ParametroBusca);
                        ListaAtividades.Clear();
                        tmp.ForEach(i => ListaAtividades.Add(i));
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ops", ex.Message, "OK");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }

        public ICommand AbrirDetalhesAtividade
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    await Shell.Current.GoToAsync(
                        $"//CadastroAtividade?parametro_id={id}");
                });
            }
        }

        public ICommand Remover
        {
            get
            {
                return new Command<int>(async (int id) =>
                {
                    try
                    {
                        bool conf = await Shell.Current.DisplayAlert(
                            "Tem Certeza?",
                            "Excluir", "Sim", "Não");

                        if (conf)
                        {
                            await App.Database.Delete(id);
                            Atualizarlista.Execute(null);
                        }
                    }
                    catch (Exception ex)
                    {
                        await Shell.Current.DisplayAlert("Ops", ex.Message, "Ok");
                    }
                    finally
                    {
                        EstaAtualizando = false;
                    }
                });
            }
        }
    }
}
