using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(RelaxationProcedureVM), nameof(RelaxationProcedureVM))]
public partial class RelaxationProcedurePage : ContentPage
{
    private RelaxationProcedureViewModel? relaxationProcedureVM;
    public RelaxationProcedureViewModel? RelaxationProcedureVM
    {
        get => relaxationProcedureVM;
        set
        {
            if (value != relaxationProcedureVM)
            {
                BindingContext = value;
                relaxationProcedureVM = value;

                OnPropertyChanged();
            }
        }
    }

    public RelaxationProcedurePage()
	{
		InitializeComponent();
	}
}