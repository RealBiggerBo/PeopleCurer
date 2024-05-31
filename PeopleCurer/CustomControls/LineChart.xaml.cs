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
                ctrl.DrawChart(ctrl.PrepareData());
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

    private static int RenderWith = 300;
    private static int RenderHeight = 300;

    private static int charWidth = 10;
    private static int charHeight = 15;
    private static int arrowSteps = 8;

    private static int barLength = 3;
    private static int numXBars = 7;
    private static int numYBars = 10;

    private DisplayMode displayMode;
    private enum DisplayMode
    {
        Week,
        Month
    }

    public LineChart()
	{
		InitializeComponent();

        displayMode = DisplayMode.Week;

        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        DrawChart(PrepareData());
	}

    private Dictionary<DateOnly, int> PrepareData()
    {
        if(Data is null)
        {
            return new Dictionary<DateOnly,int>();
        }

        Dictionary<DateOnly, int> preparedData = new Dictionary<DateOnly, int>();

        DateOnly i = DateOnly.FromDateTime(DateTime.Today.AddDays(-7));
        int lastValue = GetLastValue(i);
        while (i < DateOnly.FromDateTime(DateTime.Today))
        {
            if (Data.TryGetValue(i, out int value))
            {
                preparedData.Add(i, value);
            }
            else
            {
                preparedData.Add(i, lastValue);
            }

            i = i.AddDays(1);
        }

        if(Data.TryGetValue(DateOnly.FromDateTime(DateTime.Today),out int todayValue))
        {
            preparedData.Add(i, todayValue);
        }

        return preparedData;
    }

    private int GetLastValue(DateOnly referenceDate)
    {
        if(Data.Count == 0)
            return 50;

        List<DateOnly> datesList = Data.Keys.ToList();
        datesList.Sort(); //first item is earliest date
        for (int i = 0; i < datesList.Count; i++)
        {
            if (datesList[datesList.Count - 1 - i] < referenceDate)
            {
                return Data[datesList[datesList.Count - 1 - i]];
            }
        }
        return Data[datesList[0]];
    }

    public void DrawChart(Dictionary<DateOnly,int> data)
    {
        if (RenderWith == 0 || RenderHeight == 0)
            return;

        //create bitmap
        byte[] buffer = GetImageData(RenderWith, RenderHeight, data);

        //show image
        chartImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
    }

    private byte[] GetImageData(int width, int height, Dictionary<DateOnly, int> entries)
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
        DrawBackground(width,height, buffer.AsSpan());
        DrawChart(width, height, buffer.AsSpan(), entries);
        DrawLine(width, height, buffer.AsSpan(), entries);

        return buffer;
    }

    private static void DrawBackground(int width, int height, Span<byte> data)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int index = GetIndexFromXY(x, y, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0xFF;
                data[index + 2] = 0xFF;
            }
        }
    }

    private void DrawChart(int width, int height, Span<byte> data, Dictionary<DateOnly, int> entries)
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
            for (int y = charHeight - i; y <= charHeight + i; y++)
            {
                int index = GetIndexFromXY(width - i - 1, y, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0x00;
                data[index + 2] = 0x00;
            }
            //yAxis
            for (int x = charWidth * 3 - i; x <= charWidth * 3 + i; x++)
            {
                int index = GetIndexFromXY(x, height - i - 1, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0x00;
                data[index + 2] = 0x00;
            }
        }

        //bars
        //xAxis
        for(int i = 0; i < numXBars; i++)
        {
            int xCoord = GetXFromDataValue(i, width, height);

            for (int y = charHeight - barLength; y <= charHeight + barLength; y++)
            {
                int index = GetIndexFromXY(xCoord, y, width, height);
                data[index] = 0xFF;
                data[index + 1] = 0x00;
                data[index + 2] = 0x00;
            }

            if (entries is null)
                continue;
            if (i+ 1 < entries.Count)
            {
                Label label = new Label();
                label.Text = DateToString(entries.ElementAt(i + 1).Key);
                label.FontSize = 10;
                InitLabel(label,xCoord, height - charHeight + barLength, true);

                statisticsAbsoluteLayout.Children.Add(label);
            }
        }
        //yAxis
        for (int i = 0; i < numYBars; i++)
        {
            int yCoord = GetYFromDataValue((i + 1) * 10, width, height);

            for (int x = charWidth * 3 - barLength; x <= charWidth * 3 + barLength; x++)
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

    private static void DrawLine(int width, int height, Span<byte> data, Dictionary<DateOnly,int> entries)
    {
        if(entries is null)
            return;
        for (int i = 0; i < Math.Min(8, entries.Count); i++)
        {
            int xCoord = GetXFromDataValue(i - 1, width, height);
            int dataValue = entries.ElementAt(Math.Max(0, entries.Count - 8) + i).Value;
            int yCoord = GetYFromDataValue(dataValue, width, height);

            DrawDataPoint(xCoord, yCoord, width, height, data);
        }
    }

    private static int GetYFromDataValue(int dataValue, int width, int height)
    {
        return (int)(charHeight + (dataValue / 10f) * (height - charHeight - arrowSteps) / (numYBars + 1f));
    }

    private static int GetXFromDataValue(int dataValue, int width, int height)
    {
        return (int)(charWidth * 3 + (dataValue + 1) * (width - charWidth * 3 - arrowSteps) / (numXBars + 1f));
    }

    private static void DrawDataPoint(int x, int y, int width, int height, Span<byte> data)
    {
        int radius = 5;
        for (int dx = -radius; dx < radius; dx++)
        {
            for (int dy = -radius; dy < radius; dy++)
            {
                if (dx * dx + dy * dy > radius)
                    continue;

                int index = GetIndexFromXY(x + dx,y + dy, width, height);

                data[index] = 0x00;
                data[index++] = 0x00;
                data[index++] = 0x00;
            }
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