using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using PeopleCurer.ViewModels;

namespace PeopleCurer.Views;

//[QueryProperty(nameof(RewardValue), nameof(RewardValue))]
[QueryProperty(nameof(Lesson), nameof(Lesson))]
public partial class RewardPage : ContentPage
{
    private LessonViewModel? lesson;
    public LessonViewModel? Lesson
    {
        get => lesson;
        set
        {
            if (value != lesson)
            {
                BindingContext = value;
                lesson = value;

                //Apply reward
                oldLevel = RewardManager.GetCurrentLevel();
                oldProgress = RewardManager.GetCurrentLevelProgress();
                oldXP = RewardManager.GetCurrentXP();
                //oldRequiredLevelUpXP = RewardManager.GetRequiredLevelUpXP();
                lesson?.AddReward();
                newLevel = RewardManager.GetCurrentLevel();
                newProgress = RewardManager.GetCurrentLevelProgress();
                newXP = RewardManager.GetCurrentXP();
                //newRequiredLevelUpXP = RewardManager.GetRequiredLevelUpXP();

                //start level-up animation
                _ = StartAnimation();
            }
        }
    }

    private const uint animationLenth = 2000; //in ms
    private float oldProgress;
    private int oldLevel;
    private int oldXP;
    //private int oldRequiredLevelUpXP;
    private float newProgress;
    private int newLevel;
    private int newXP;
    //private int newRequiredLevelUpXP;

    private int curAnimationLevel;

	public RewardPage()
	{
		InitializeComponent();
	}

    private async Task StartAnimation()
    {
        curAnimationLevel = oldLevel;

        xpBar.Progress = oldProgress;

        progressLabel.Text = oldXP + "/" + RewardManager.GetRequiredLevelUpXP();

        for (int i = oldLevel; i < newLevel; i++)
        {
            await xpBar.ProgressTo(1, animationLenth, Easing.SinIn);
            await Task.Delay(500);
            curAnimationLevel = i + 1;
            xpBar.Progress = 0;

            //TODO: set level text box
        }

        await xpBar.ProgressTo(newProgress, animationLenth, Easing.SinInOut);
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private void xpBar_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if(e.PropertyName == nameof(ProgressBar.Progress))
        {
            double totalPercentage = newProgress - oldProgress + 100 * (newLevel - oldLevel);

            double percentageLeft = newProgress - xpBar.Progress + 100 * (newLevel - curAnimationLevel);

            double progress = 1 - (percentageLeft / totalPercentage);

            progressLabel.Text = (int)double.Lerp(oldXP, newXP, progress) + "/" + RewardManager.GetRequiredLevelUpXP();
        }
    }
}