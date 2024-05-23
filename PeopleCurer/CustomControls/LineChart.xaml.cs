using Android.Views;
using Microsoft.Maui.Controls.Shapes;
using PeopleCurer.Models;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace PeopleCurer.CustomControls;

public partial class LineChart : ContentView
{
    public static readonly BindableProperty DataProperty =
        BindableProperty.Create(nameof(Data), typeof(Dictionary<DateOnly, int>), typeof(LineChart),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                LineChart ctrl = (LineChart)bindable;
                ctrl.DrawChart();
                //ctrl.dataPath.Data = await Task.Run(() => ctrl.GetDataGeometry(newVal));
            });

    public static readonly BindableProperty YAxisTextFontSizeProperty =
        BindableProperty.Create(nameof(YAxisTextFontSize), typeof(double), typeof(LineChart), 15d);

    public static readonly BindableProperty XAxisTextFontSizeProperty =
        BindableProperty.Create(nameof(XAxisTextFontSize), typeof(double), typeof(LineChart), 15d);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LineChart));

    public static readonly BindableProperty AxisColorProperty =
        BindableProperty.Create(nameof(AxisColor), typeof(Color), typeof(LineChart));

    public static readonly BindableProperty DataColorProperty =
        BindableProperty.Create(nameof(DataColor), typeof(Color), typeof(LineChart));

    public Dictionary<DateOnly, int> Data
	{
		get => (Dictionary<DateOnly, int>)GetValue(DataProperty);
		set => SetValue(DataProperty, value);
	}

    public double YAxisTextFontSize
    {
        get => (double)GetValue(YAxisTextFontSizeProperty);
        set => SetValue(YAxisTextFontSizeProperty, value);
    }

    public double XAxisTextFontSize
    {
        get => (double)GetValue(XAxisTextFontSizeProperty);
        set => SetValue(XAxisTextFontSizeProperty, value);
    }

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    public Color AxisColor
    {
        get => (Color)GetValue(AxisColorProperty);
        set => SetValue(AxisColorProperty, value);
    }

    public Color DataColor
    {
        get => (Color)GetValue(DataColorProperty);
        set => SetValue(DataColorProperty, value);
    }

    Vector2 startingPoint;
    Vector2 axisLength;
    Vector2 barCounts = new Vector2(7, 10);
    Vector2 barOffsets;
    Vector2 barDimensions = new Vector2(6,6);
    Vector2 textSpace = new Vector2(30, 30);

    private int RenderWith = 300;
    private int RenderHeight = 300;

    private static int charWidth = 10;
    private static int charHeight = 15;
    private static int arrowSteps = 8;

	public LineChart()
	{
		InitializeComponent();

        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        DrawChart();
	}

    public void DrawChart()
    {
        if (RenderWith == 0 || RenderHeight == 0)
            return;

        //create bitmap
        byte[] buffer = GetData(RenderWith, RenderHeight, Data);

        //update text
        //while (stackLayout.Children.Count > 1)
        //{
        //    stackLayout.Children.RemoveAt(1);
        //}

        //for (int x = 0; x < Data.Count; x++)
        //{
        //    HorizontalStackLayout horizontalLayout = new HorizontalStackLayout();
        //    horizontalLayout.VerticalOptions = LayoutOptions.Center;
        //    horizontalLayout.Spacing = 5;

        //    Rectangle rect = new Rectangle();
        //    rect.BackgroundColor = Color.FromHsv((float)x / Data.Count, 1, 1);
        //    rect.WidthRequest = 20;
        //    rect.HeightRequest = 20;
        //    horizontalLayout.Children.Add(rect);

        //    Label zeroLabel = new Label();
        //    zeroLabel.Text = Data[x].thoughtName;
        //    horizontalLayout.Children.Add(zeroLabel);

        //    stackLayout.Children.Add(horizontalLayout);
        //}

        //show image
        chartImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
    }

    private byte[] GetData(int width, int height, Dictionary<DateOnly, int> data)
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
        IntToBytes(dataSize, out byte dataSizeA, out byte dataSizeB, out byte dataSizeC, out byte dataSizeD);

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
        DrawChart(width, height, buffer.AsSpan<byte>());
        DrawLine(width, height, buffer.AsSpan());

        return buffer;
    }

    private void DrawChart(int width, int height, Span<byte> data)
    {
        //xAxis
        for (int x = charWidth * 3; x < width; x++)
        {
            int index = GetIndexFromXY(x, charHeight, width, height);
            data[index] = 0xFF;
            data[index + 1] = 0x00;
            data[index + 2] = 0x00;
        }
        //yAxis
        for (int y = charHeight; y < height; y++)
        {
            int index = GetIndexFromXY(charWidth * 3,y, width, height);
            data[index] = 0xFF;
            data[index + 1] = 0x00;
            data[index + 2] = 0x00;
        }

        //arrows
        for (int i = 0; i < arrowSteps; i++)
        {
            //xAxis
            for (int y = charHeight - i; y < charHeight + i; y++)
            {
                int index = GetIndexFromXY(width - i - 1, y, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0x00;
                data[index + 2] = 0x00;
            }
            //yAxis
            for (int x = charWidth * 3 - i; x < charWidth * 3 + i; x++)
            {
                int index = GetIndexFromXY(x, height - i -1, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0x00;
                data[index + 2] = 0x00;
            }
        }

        //bars
        int barLength = 3;
        int numXBars = 7;
        int numYBars = 10;

        //xAxis
        for(int i = 0; i < numXBars; i++)
        {
            int xCoord = (int)(charWidth * 3 + (i + 1) * (width - charWidth * 3 - arrowSteps) / (numXBars + 1f));

            for (int y = charHeight - barLength; y < charHeight + barLength; y++)
            {
                int index = GetIndexFromXY(xCoord, y, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0x00;
                data[index + 2] = 0x00;
            }

            if (Data is null)
                break;
            if (Data.Count - i - 1 >= 0)
            {

                Label label = new Label();
                label.Text = DateToString(Data.ElementAt(Data.Count - i - 1).Key);
                label.FontSize = 10;
                InitLabel(label,xCoord, height - charHeight + barLength, true);

                statisticsAbsoluteLayout.Children.Add(label);
            }
        }
        //yAxis
        for (int i = 0; i < numYBars; i++)
        {
            int yCoord = (int)(charHeight + (i + 1) * (height - charHeight - arrowSteps) / (numYBars + 1f));

            for (int x = charWidth * 3 - barLength; x < charWidth * 3 + barLength; x++)
            {
                int index = GetIndexFromXY(x, yCoord, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0x00;
                data[index + 2] = 0x00;
            }

            Label label = new Label();
            label.Text = ((i + 1) * 10).ToString();
            InitLabel(label, 0, height - yCoord, false);

            statisticsAbsoluteLayout.Children.Add(label);
        }

        //add zero label
        Label zeroLabel = new Label();
        zeroLabel.Text = "0";
        InitLabel(zeroLabel, 0, height - charHeight, false);

        statisticsAbsoluteLayout.Children.Add(zeroLabel);
    }

    private void InitLabel(Label label, double xCoord, double yCoord, bool alignWithXCoord)
    {
        void SizeChanged(object? obj, EventArgs e)
        {
            label.TranslationX = xCoord - (alignWithXCoord ? label.Width / 2 : 0);
            label.TranslationY = yCoord - (!alignWithXCoord ? label.Height / 2 : 0);

            label.SizeChanged -= SizeChanged;
        }

        label.SizeChanged += SizeChanged;
    }

    private void DrawLine(int width, int height, Span<byte> data)
    {
        if(Data is null)
            return;
        for (int i = 0; i < Math.Min(7, Data.Count); i++)
        {
            int xCoord = (int)(charWidth * 3 + (i + 1) * (width - charWidth * 3 - arrowSteps) / (8f));
            int dataValue = Data.ElementAt(Math.Max(0,Data.Count - 8) + i).Value;
            int yCoord = (int)(charHeight + (dataValue / 10f + 1) * (height - charHeight - arrowSteps) / (11f)); ;

            int index = GetIndexFromXY(xCoord, yCoord , width, height);

            data[index] = 0xFF;
            data[index++] = 0xFF;
            data[index++] = 0xFF;
        }
    }

    private static int GetIndexFromXY(int x, int y, int width, int height)
    {
        int startIndex = 54;
        int paddingCount = width % 4;
        return startIndex + 3 * y * width + y * paddingCount + x * 3;
    }

    private static void IntToBytes(int integer, out byte a, out byte b, out byte c, out byte d)
    {
        a = (byte)(integer & 0x000000ff);
        b = (byte)((integer & 0x0000ff00) >> 8);
        c = (byte)((integer & 0x00ff0000) >> 16);
        d = (byte)((integer & 0xff000000) >> 24);
    }

    private static string DateToString(DateOnly date)
    {
        return date.Day + "." + date.Month;
    }
}