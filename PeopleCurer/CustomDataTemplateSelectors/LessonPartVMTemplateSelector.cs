using PeopleCurer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleCurer.CustomDataTemplateSelectors
{
    sealed class LessonPartVMTemplateSelector : DataTemplateSelector
    {
        public DataTemplate InfoPageTemplate { get; set; }
        public DataTemplate QuestionTemplate { get; set; }
        public DataTemplate EvaluationTemplate { get; set; }
        public DataTemplate SymptomCheckQuestionTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if(item is InfoPageViewModel)
            {
                return InfoPageTemplate;
            }
            else if(item is QuestionViewModel)
            {
                return QuestionTemplate;
            }
            else if(item is SymptomCheckQuestionViewModel)
            {
                return SymptomCheckQuestionTemplate;
            }
            else //if(item is EvaluationViewModel)
            {
                return EvaluationTemplate;
            }
        }
    }
}
