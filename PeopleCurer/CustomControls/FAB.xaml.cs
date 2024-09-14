using PeopleCurer.Views;

namespace PeopleCurer.CustomControls;

public partial class FAB : ContentView
{
    public static readonly BindableProperty FABContentProperty =
        BindableProperty.Create(nameof(FABContent), typeof(View), typeof(FAB),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                ((FAB)bindable).presenter.Content = (View)newVal;
            });

    public View FABContent
    {
        get => (View)GetValue(FABContentProperty);
        set => SetValue(FABContentProperty, value);
    }

    public FAB()
	{
		InitializeComponent();
	}

    private void DropGestureRecognizer_Drop(object sender, DropEventArgs e)
    {
        Point? pos = e.GetPosition(this);
        if(pos is Point p)
        {
            p.X = double.Clamp(p.X, dragElement.Width / 2, this.Width - dragElement.Width / 2);
            p.Y = double.Clamp(p.Y, dragElement.Height / 2, this.Height - dragElement.Height / 2);

            p = p.Offset(-dragElement.Width / 2, -dragElement.Height / 2);


            Rect bounds = new Rect(p.X, p.Y, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize);

            AbsoluteLayout.SetLayoutBounds(dragElement, bounds);
        }
    }

    private void DragGestureRecognizer_DragStarting(object sender, DragStartingEventArgs e)
    {
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ChatBotPage));
    }
}