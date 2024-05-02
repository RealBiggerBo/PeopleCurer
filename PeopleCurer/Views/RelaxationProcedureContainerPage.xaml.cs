using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(RelaxationProcedureContainerVM),nameof(RelaxationProcedureContainerVM))]
public partial class RelaxationProcedureContainerPage : ContentPage
{
    private RelaxationProcedureContainerViewModel? relaxationProcedureContainerVM;
    public RelaxationProcedureContainerViewModel? RelaxationProcedureContainerVM
    {
        get => relaxationProcedureContainerVM;
        set
        {
            if (value != relaxationProcedureContainerVM)
            {
                BindingContext = value;
                relaxationProcedureContainerVM = value;

                OnPropertyChanged();
            }
        }
    }

    public RelaxationProcedureContainerPage()
	{
		InitializeComponent();
	}
}