using PeopleCurer.Models;
using PeopleCurer.MVVMHelpers;
using PeopleCurer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.ViewModels
{
    public sealed class RelaxationProcedureViewModel : NotifyableBaseObject
    {
        public readonly RelaxationProcedure relaxationProcedure;

        public string Title { get => relaxationProcedure.title; }

        public string Explanation { get => relaxationProcedure.explanation; }

        public string Effectiveness { get => relaxationProcedure.effectiveness; }

        public DelegateCommand GoToRelaxationProcedurePage { get; }

        public RelaxationProcedureViewModel(RelaxationProcedure relaxationProcedure)
        {
            this.relaxationProcedure = relaxationProcedure;

            GoToRelaxationProcedurePage = new DelegateCommand((obj) => Shell.Current.GoToAsync(nameof(RelaxationProcedurePage),
                    new Dictionary<string, object>
                    {
                        ["RelaxationProcedureVM"] = this
                    }));
        }
    }
}
