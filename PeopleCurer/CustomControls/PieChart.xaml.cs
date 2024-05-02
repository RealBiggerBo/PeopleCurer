using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics.Platform;
using PeopleCurer.Models;
using PeopleCurer.ViewModels;
using System.Numerics;

namespace PeopleCurer.CustomControls;

public partial class PieChart : ContentView
{
    public static readonly BindableProperty DataProperty =
        BindableProperty.Create(nameof(Data), typeof(List<Thought>), typeof(PieChart),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                PieChart ctrl = (PieChart)bindable;
				ctrl.DrawChart();
            });

	public static readonly BindableProperty RenderWithProperty =
		BindableProperty.Create(nameof(RenderWith), typeof(int), typeof(PieChart), 0);

    public static readonly BindableProperty RenderHeightProperty =
        BindableProperty.Create(nameof(RenderHeight), typeof(int), typeof(PieChart), 0);

    public List<Thought> Data
    {
        get => (List<Thought>)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

	public int RenderWith
	{
		get => (int)GetValue(RenderWithProperty);
		set => SetValue(RenderWithProperty, value);
	}

    public int RenderHeight
    {
        get => (int)GetValue(RenderHeightProperty);
        set => SetValue(RenderHeightProperty, value);
    }

    public PieChart()
	{
		InitializeComponent();

		DrawChart();
	}

	public void DrawChart()
	{
		if (Data is null || RenderWith == 0 || RenderHeight == 0)
			return;

		//create bitmap
		byte[] buffer = GetData(RenderWith, RenderHeight, Data);

		//update text
		while (stackLayout.Children.Count > 1) 
		{
			stackLayout.Children.RemoveAt(1);
		}

		for (int i = 0; i < Data.Count; i++)
		{
			HorizontalStackLayout horizontalLayout = new HorizontalStackLayout();
			horizontalLayout.VerticalOptions = LayoutOptions.Center;
			horizontalLayout.Spacing = 5;

			Rectangle rect = new Rectangle();
			rect.BackgroundColor = Color.FromHsv((float)i / Data.Count, 1, 1);
			rect.WidthRequest = 20;
			rect.HeightRequest = 20;
			horizontalLayout.Children.Add(rect);

			Label label = new Label();
			label.Text = Data[i].thoughtName;
			horizontalLayout.Children.Add(label);

			stackLayout.Children.Add(horizontalLayout);
		}
		
		//show image
		chartImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
	}

	private static byte[] GetData(int width, int height, List<Thought> data)
	{
		//compute buffer length
		int paddingCountPerRow = width % 4;
		int dataSize = 3 * width * height + paddingCountPerRow * height;
        byte[] buffer = new byte[14 + 40 + dataSize];

		//header
		IntToBytes(buffer.Length, out byte bufferLengthA, out byte bufferLengthB, out byte bufferLengthC, out byte bufferLengthD);

		buffer[0] = 0x42;
		buffer[1] = 0x4D;
		buffer[2] = bufferLengthA; //num bytes
		buffer[3] = bufferLengthB; //
		buffer[4] = bufferLengthC; //
		buffer[5] = bufferLengthD; //
		buffer[6] = 0x00;
		buffer[7] = 0x00;
		buffer[8] = 0x00;
		buffer[9] = 0x00;
		buffer[10] = 0x36;
		buffer[11] = 0x00;
		buffer[12] = 0x00;
		buffer[13] = 0x00;

		//dib
		IntToBytes(width, out byte widthA, out byte widthB, out byte widthC, out byte widthD);
		IntToBytes(height, out byte heightA, out byte heightB, out byte heightC, out byte heightD);
		IntToBytes(dataSize , out byte dataSizeA, out byte dataSizeB, out byte dataSizeC, out byte dataSizeD);

        buffer[14] = 0x28;
		buffer[15] = 0x00;
		buffer[16] = 0x00;
		buffer[17] = 0x00;
		buffer[18] = widthA; //width
		buffer[19] = widthB; //
		buffer[20] = widthC; //
		buffer[21] = widthD; //
		buffer[22] = heightA; //height
		buffer[23] = heightB; //
		buffer[24] = heightC; //
		buffer[25] = heightD; //
		buffer[26] = 0x01;
		buffer[27] = 0x00;
		buffer[28] = 0x18;
		buffer[29] = 0x00;
		buffer[30] = 0x00;
		buffer[31] = 0x00;
		buffer[32] = 0x00;
		buffer[33] = 0x00;
		buffer[34] = dataSizeA; //size of pixel buffer
		buffer[35] = dataSizeB; //
		buffer[36] = dataSizeC; //
		buffer[37] = dataSizeD; //
		buffer[38] = 0x13;
		buffer[39] = 0x0B;
		buffer[40] = 0x00;
		buffer[41] = 0x00;
        buffer[42] = 0x13;
        buffer[43] = 0x0B;
        buffer[44] = 0x00;
        buffer[45] = 0x00;
        buffer[46] = 0x00;
        buffer[47] = 0x00;
        buffer[48] = 0x00;
        buffer[49] = 0x00;
        buffer[50] = 0x00;
        buffer[51] = 0x00;
        buffer[52] = 0x00;
        buffer[53] = 0x00;

		//pixel buffer
		int circleRadius = (int)MathF.Min(width / 2, height / 2);

		int index = 54;
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Vector2 uv = new Vector2(x,y);

				byte r = 0xFF;
				byte g = 0x00;
				byte b = 0xFF;

				Vector2 vec = uv - new Vector2(width / 2, height / 2);

                if (vec.LengthSquared() <= circleRadius * circleRadius)
				{
					//inside circle
					GetColorFromVec(vec, data, out r, out g, out b);
				}
				else
				{
					//background color
					r = 0xFF; g = 0xFF; b = 0xFF;
				}


				//set pixel byte buffer
				buffer[index] = b;//blue
				index++;
				buffer[index] = g;//green
				index++;
				buffer[index] = r;//red
				index++;
			}

			for (int i = 0; i < paddingCountPerRow; i++)
			{
				buffer[index] = 0x00;
				index++;
			}
		}

        return buffer;
	}

    private static void GetColorFromVec(Vector2 vec, List<Thought> data, out byte r, out byte g, out byte b)
    {
		int total = 0;
		for (int i = 0; i < data.Count; i++)
		{
			total += data[i].thoughtProbability;
		}

		float vecAngle = 0.5f*MathF.PI - MathF.Atan(vec.Y / vec.X) + (vec.X < 0 ? MathF.PI : 0);
		float angle = 0;
		for (int i = 0; i < data.Count; i++)
		{
			angle += ((float)data[i].thoughtProbability / total) * MathF.PI * 2;

			if (angle > vecAngle)
			{
				Color col = Color.FromHsv((float)i / data.Count, 1, 1);

				r = (byte)(col.Red * 255);
				g = (byte)(col.Green * 255);
				b = (byte)(col.Blue * 255);
				return;
			}
		}

		r = 0xFF;
		g = 0x00;
		b = 0x00;
    }

    private static void IntToBytes(int integer, out byte a, out byte b, out byte c, out byte d)
	{
		a = (byte)(integer & 0x000000ff);
		b = (byte)((integer & 0x0000ff00) >> 8);
        c = (byte)((integer & 0x00ff0000) >> 16);
        d = (byte)((integer & 0xff000000) >> 24);
    }
}