using PeopleCurer.MVVMHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    sealed class RewardViewModel : NotifyableBaseObject
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
                    base.RaisePropertyChanged();
                }
            }
        }

        public DelegateCommand GoBackCMD { get; }

        public RewardViewModel()
        {
            GoBackCMD = new DelegateCommand(async (_) => await Shell.Current.GoToAsync(".."));
        }
    }
}
