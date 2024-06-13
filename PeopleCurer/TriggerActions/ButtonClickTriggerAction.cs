using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.TriggerActions
{
    internal sealed class ButtonClickTriggerAction : TriggerAction<Button>
    {
        protected override void Invoke(Button button)
        {
            const uint length = 100;
            const float minValue = 0.8f;
            const float a = 4 * (1 - minValue) / (length * length);
            button.Animate("buttonPressAnimation", (d) => button.Opacity = a * Math.Pow(d*length - length / 2,2) + minValue,16, length,null,(d,b) => button.Opacity = 1);
        }
    }
}
