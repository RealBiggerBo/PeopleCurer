using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
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
                oldLevel = RewardManager.GetCurrentLevel();
                oldProgress = RewardManager.GetCurrentLevelProgress();
                ((RewardPageViewModel)this.BindingContext).RewardValue = value;
                newLevel = RewardManager.GetCurrentLevel();
                newProgress = RewardManager.GetCurrentLevelProgress();

                _ = StartAnimation();
            }
        }
    }

    private const uint animationLenth = 2000; //in ms
    private float oldProgress;
    private int oldLevel;
    private float newProgress;
    private int newLevel;

	public RewardPage()
	{
		InitializeComponent();
	}

    private async Task StartAnimation()
    {
        xpBar.Progress = oldProgress;

        for (int i = oldLevel; i < newLevel; i++)
        {
            await xpBar.ProgressTo(1, animationLenth, Easing.SinIn);
            xpBar.Progress = 0;
            await Task.Delay(1000);

            //set level text box
        }

        await xpBar.ProgressTo(newProgress, animationLenth, Easing.SinInOut);
    }
}