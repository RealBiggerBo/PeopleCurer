using PeopleCurer.Views;
using System.Diagnostics;

namespace PeopleCurer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(WelcomePage), typeof(WelcomePage));
            Routing.RegisterRoute(nameof(CoursePage), typeof(CoursePage));
            Routing.RegisterRoute(nameof(LessonPage), typeof(LessonPage));
            Routing.RegisterRoute(nameof(BehaviourExperimentPage), typeof(BehaviourExperimentPage));
            Routing.RegisterRoute(nameof(SituationEditPage), typeof(SituationEditPage));
            Routing.RegisterRoute(nameof(SituationFinishPage), typeof(SituationFinishPage));
            Routing.RegisterRoute(nameof(SituationCompletedPage), typeof(SituationCompletedPage));
            Routing.RegisterRoute(nameof(ThoughtTestContainerPage), typeof(ThoughtTestContainerPage));
            Routing.RegisterRoute(nameof(ThoughtTestEditPage), typeof(ThoughtTestEditPage));
            Routing.RegisterRoute(nameof(ThoughtTestFinishPage), typeof(ThoughtTestFinishPage));
            Routing.RegisterRoute(nameof(ThoughtTestCompletedPage), typeof(ThoughtTestCompletedPage));
            Routing.RegisterRoute(nameof(RelaxationProcedureContainerPage), typeof(RelaxationProcedureContainerPage));
            Routing.RegisterRoute(nameof(RelaxationProcedurePage), typeof(RelaxationProcedurePage));
            Routing.RegisterRoute(nameof(RewardPage), typeof(RewardPage));
        }
    }
}
