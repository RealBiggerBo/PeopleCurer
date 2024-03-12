using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.CustomEventArgs
{
    public class AnswerEventArgs : EventArgs
    {
        public bool IsChoses { get; set; }

        public AnswerEventArgs(bool isChosen)
        {
            this.IsChoses = isChosen;
        }
    }
}
