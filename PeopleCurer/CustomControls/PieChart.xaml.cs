using Microsoft.Maui.Graphics.Platform;

namespace PeopleCurer.CustomControls;

public partial class PieChart : ContentView
{
	public PieChart()
	{
		InitializeComponent();

		DrawChart();
	}

	public void DrawChart()
	{
		ImagePaint a = new ImagePaint();
		chartImage.Source = ImageSource.FromStream(() => new MemoryStream(new byte[] { 128, 128, 128, 128 }));
	}
}