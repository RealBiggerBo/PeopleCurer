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
        public DataTemplate ThoughtTestLessonPart1Template { get; set; }
        public DataTemplate ThoughtTestLessonPart2Template { get; set; }
        public DataTemplate ThoughtTestLessonPart3Template { get; set; }
        public DataTemplate ThoughtTestLessonPart4Template { get; set; }
        public DataTemplate SituationLessonPartCreate1Template { get; set; }
        public DataTemplate SituationLessonPartCreate2Template { get; set; }
        public DataTemplate SituationLessonPartCreate3Template { get; set; }
        public DataTemplate SituationLessonPartFinish1Template { get; set; }
        public DataTemplate SituationLessonPartFinish2Template { get; set; }
        public DataTemplate SituationLessonPartFinish3Template { get; set; }
        public DataTemplate SituationLessonPartFinish4Template { get; set; }

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
            else if(item is EvaluationViewModel)
            {
                return EvaluationTemplate;
            }
            else if (item is ThoughtTestLessonPart1ViewModel)
            {
                return ThoughtTestLessonPart1Template;
            }
            else if (item is ThoughtTestLessonPart2ViewModel)
            {
                return ThoughtTestLessonPart2Template;
            }
            else if (item is ThoughtTestLessonPart3ViewModel)
            {
                return ThoughtTestLessonPart3Template;
            }
            else if (item is ThoughtTestLessonPart4ViewModel)
            {
                return ThoughtTestLessonPart4Template;
            }
            else if(item is SituationLessonPartCreate1ViewModel)
            {
                return SituationLessonPartCreate1Template;
            }
            else if (item is SituationLessonPartCreate2ViewModel)
            {
                return SituationLessonPartCreate2Template;
            }
            else if (item is SituationLessonPartCreate3ViewModel)
            {
                return SituationLessonPartCreate3Template;
            }
            else if (item is SituationLessonPartFinish1ViewModel)
            {
                return SituationLessonPartFinish1Template;
            }
            else if (item is SituationLessonPartFinish2ViewModel)
            {
                return SituationLessonPartFinish2Template;
            }
            else if (item is SituationLessonPartFinish3ViewModel)
            {
                return SituationLessonPartFinish3Template;
            }
            else if(item is SituationLessonPartFinish4ViewModel)
            {
                return SituationLessonPartFinish4Template;
            }
            else
            {
                return InfoPageTemplate;
            }
        }
    }
}
