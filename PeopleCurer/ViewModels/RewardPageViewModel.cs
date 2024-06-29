using PeopleCurer.MVVMHelpers;
using PeopleCurer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    sealed class RewardPageViewModel : NotifyableBaseObject
    {
        private int rewardValue;
        public int RewardValue
        {
            get => rewardValue;
            set
            {
                if(rewardValue != value)
                {
                    rewardValue = value;
                    RewardManager.AddRewardXP(rewardValue);
                    base.RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand GoBackCMD { get; }

        public RewardPageViewModel()
        {
            GoBackCMD = new DelegateCommand(async (_) => await Shell.Current.GoToAsync(".."));
        }
    }
}
