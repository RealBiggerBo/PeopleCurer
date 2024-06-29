using PeopleCurer.MVVMHelpers;
using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

[QueryProperty(nameof(RewardValue), nameof(RewardValue))]
public partial class RewardPage : ContentPage
{
	public int RewardValue
    {
        get => ((RewardPageViewModel)this.BindingContext).RewardValue;
        set
        {
            if (value != ((RewardPageViewModel)this.BindingContext).RewardValue)
            {
                ((RewardPageViewModel)this.BindingContext).RewardValue = value;
            }
        }
    }

	public RewardPage()
	{
		InitializeComponent();
	}
}