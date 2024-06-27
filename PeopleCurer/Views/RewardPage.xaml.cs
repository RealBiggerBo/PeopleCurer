using PeopleCurer.MVVMHelpers;
using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(RewardValue), nameof(RewardValue))]
public partial class RewardPage : ContentPage
{
	public int RewardValue
    {
        get => ((RewardViewModel)this.BindingContext).RewardValue;
        set
        {
            if (value != ((RewardViewModel)this.BindingContext).RewardValue)
            {
                ((RewardViewModel)this.BindingContext).RewardValue = value;

                OnPropertyChanged();
            }
        }
    }

	public RewardPage()
	{
		InitializeComponent();
	}
}