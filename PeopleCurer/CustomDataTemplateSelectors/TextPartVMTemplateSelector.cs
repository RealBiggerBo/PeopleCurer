using PeopleCurer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.CustomDataTemplateSelectors
{
    sealed class TextPartVMTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TextBlockTemplate { get; set; }
        public DataTemplate EnumerationTemplate { get; set; }
        public DataTemplate TextBoxTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(item is TextBlockViewModel)
            {
                return TextBlockTemplate;
            }
            else if(item is EnumerationViewModel)
            {
                return EnumerationTemplate;
            }
            else if(item is TextBoxViewModel)
            {
                return TextBoxTemplate;
            }
            else
            {
                return TextBlockTemplate;
            }
        }
    }
}
