using PeopleCurer.Services;
using System.Numerics;

namespace PeopleCurer.CustomControls;

public partial class LineChart : ContentView
{
    public static readonly BindableProperty DataProperty =
        BindableProperty.Create(nameof(Data), typeof(Dictionary<DateOnly, int>), typeof(LineChart),
            propertyChanged: async (bindable, oldVal, newVal) =>
            {
                LineChart ctrl = (LineChart)bindable;
                await ctrl.DrawChart();
            });

    public static readonly BindableProperty YAxisTextFontSizeProperty =
        BindableProperty.Create(nameof(YAxisTextFontSize), typeof(double), typeof(LineChart), 15d);

    public static readonly BindableProperty XAxisTextFontSizeProperty =
        BindableProperty.Create(nameof(XAxisTextFontSize), typeof(double), typeof(LineChart), 10d);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(LineChart), Color.FromRgb(0, 0, 0), 
            BindingMode.OneTime,
            propertyChanged: async (bindable, oldVal, newVal) =>
            {
                LineChart ctrl = (LineChart)bindable;
                await ctrl.DrawChart();
            });

    public static readonly BindableProperty AxisColorProperty =
        BindableProperty.Create(nameof(AxisColor), typeof(Color), typeof(LineChart), Color.FromRgb(1, 0, 1),
            BindingMode.OneTime,
            propertyChanged: async (bindable, oldVal, newVal) =>
            {
                LineChart ctrl = (LineChart)bindable;
                await ctrl.DrawChart();
            });

    public static readonly BindableProperty DataColorProperty =
        BindableProperty.Create(nameof(DataColor), typeof(Color), typeof(LineChart), Color.FromRgb(1,0,1), 
            BindingMode.OneTime,
            propertyChanged: async (bindable, oldVal, newVal) =>
            {
                LineChart ctrl = (LineChart)bindable;
                await ctrl.DrawChart();
            });

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

    private static readonly int RenderWidth = 300;
    private static readonly int RenderHeight = 300;

    private static readonly int charWidth = 10;
    private static readonly int charHeight = 15;
    private static readonly int arrowSteps = 8;

    private static readonly int barLength = 3;
    private static readonly int numXBars_Week = 7;
    private static readonly int numXBars_HalfYear = 6;
    private static readonly int numYBars = 10;

    private readonly List<Label> displayedXLabels;
    private readonly List<Label> displayedYLabels;

    private DisplayMode graphDisplayMode = DisplayMode.Week;

    private enum DisplayMode
    {
        Week,
        HalfYear
    }

    public LineChart()
	{
		InitializeComponent();

        displayedXLabels = new List<Label>();
        displayedYLabels = new List<Label>();

        ProgressUpdateManager.OnSymptomCheckQuestionCompleted += async (obj, e) => await DrawChart();
	}

    private Dictionary<DateOnly, int> PrepareData()
    {
        if(Data is null)
        {
            return new Dictionary<DateOnly,int>();
        }

        Dictionary<DateOnly, int> preparedData = new Dictionary<DateOnly, int>();

        switch (graphDisplayMode)
        {
            case DisplayMode.Week:
                {
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

                    if (Data.TryGetValue(DateOnly.FromDateTime(DateTime.Today), out int todayValue))
                    {
                        preparedData.Add(i, todayValue);
                    }
                }
                break;
            case DisplayMode.HalfYear:
                {
                    DateOnly i = DateOnly.FromDateTime(DateTime.Today.AddMonths(-6));
                    DateOnly monthKey = i.AddMonths(1).AddDays(-1 * i.Day); //last day of previous month
                    i = i.AddDays(-1 * i.Day + 1);
                    int lastValue = GetLastValue(i);
                    int totalSum = 0;
                    int averagedValues = 0;
                    while(true)
                    {
                        if(i > monthKey || i > DateOnly.FromDateTime(DateTime.Today))
                        {
                            if(averagedValues > 0)
                            {
                                preparedData.Add(monthKey, totalSum / averagedValues);
                                lastValue = totalSum / averagedValues;
                            }
                            else
                            {
                                //no values in that month
                                preparedData.Add(monthKey, lastValue);
                            }
                            totalSum = 0;
                            averagedValues = 0;

                            //get last day of next month
                            monthKey = monthKey.AddMonths(2);
                            monthKey = monthKey.AddDays(-1 * monthKey.Day);
                            ////


                            if (preparedData.Count >= 7)
                                break;
                        }

                        if(Data.TryGetValue(i, out int value))
                        {
                            totalSum += value;
                            averagedValues++;
                        }

                        i = i.AddDays(1);
                    }
                }
                break;
        }
        return preparedData;
    }

    private int GetLastValue(DateOnly referenceDate)
    {
        if (Data.Count == 0)
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

    private async Task DrawChart()
    {
        if (RenderWidth == 0 || RenderHeight == 0)
            return;

        Dictionary<DateOnly, int> preparedData = await Task.Run(PrepareData);

        //create bitmap
        byte[] buffer = await Task.Run(() => GetImageData(RenderWidth, RenderHeight, preparedData));

        DisplayXLabels(RenderWidth, RenderHeight, preparedData);
        DisplayYLabels(RenderWidth, RenderHeight);

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
        DrawChartAxis(width, height, buffer.AsSpan(), entries);
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

    private void DrawChartAxis(int width, int height, Span<byte> data, Dictionary<DateOnly, int> entries)
    {
        //xAxis
        for (int x = charWidth * 3; x < width; x++)
        {
            int index = GetIndexFromXY(x, charHeight, width, height);
            AxisColor.ToRgb(out data[index + 2], out data[index + 1], out data[index + 0]);
        }
        //yAxis
        for (int y = charHeight; y < height; y++)
        {
            int index = GetIndexFromXY(charWidth * 3,y, width, height);
            AxisColor.ToRgb(out data[index + 2], out data[index + 1], out data[index + 0]);
        }

        //arrows
        for (int i = 0; i < arrowSteps; i++)
        {
            //xAxis
            for (int y = charHeight - i; y <= charHeight + i; y++)
            {
                int index = GetIndexFromXY(width - i - 1, y, width, height);
                AxisColor.ToRgb(out data[index + 2], out data[index + 1], out data[index + 0]);
            }
            //yAxis
            for (int x = charWidth * 3 - i; x <= charWidth * 3 + i; x++)
            {
                int index = GetIndexFromXY(x, height - i - 1, width, height);
                AxisColor.ToRgb(out data[index + 2], out data[index + 1], out data[index + 0]);
            }
        }

        //xAxis bars
        int numXBars = graphDisplayMode == DisplayMode.Week ? numXBars_Week : numXBars_HalfYear;
        for(int i = 0; i < numXBars; i++)
        {
            int xCoord = GetXFromDataValue(i, width, height);

            for (int y = charHeight - barLength; y <= charHeight + barLength; y++)
            {
                int index = GetIndexFromXY(xCoord, y, width, height);
                AxisColor.ToRgb(out data[index + 2], out data[index + 1], out data[index + 0]);
            }
        }
        //yAxis bars
        for (int i = 0; i < numYBars; i++)
        {
            int yCoord = GetYFromDataValue((i + 1) * 10, width, height);

            for (int x = charWidth * 3 - barLength; x <= charWidth * 3 + barLength; x++)
            {
                int index = GetIndexFromXY(x, yCoord, width, height);
                AxisColor.ToRgb(out data[index + 2], out data[index + 1], out data[index + 0]);
            }
        }        
    }

    private void DisplayXLabels(int width, int height, Dictionary<DateOnly, int> entries)
    {
        for (int i = 0; i < displayedXLabels.Count; i++)
        {
            statisticsAbsoluteLayout.Children.Remove(displayedXLabels[i]);
        }
        displayedXLabels.Clear();

        int numXBars = graphDisplayMode == DisplayMode.Week ? numXBars_Week : numXBars_HalfYear;
        for (int x = 0; x < Math.Min(numXBars, entries.Count - 1); x++)
        {
            int xCoord = GetXFromDataValue(x, width, height);

            Label label = new Label();
            label.Text = DateToString(entries.ElementAt(x + 1).Key, graphDisplayMode);
            label.TextColor = TextColor;
            label.FontSize = XAxisTextFontSize;
            InitLabel(label, xCoord ,height - charHeight + barLength, true);

            displayedXLabels.Add(label);
            statisticsAbsoluteLayout.Children.Add(label);
        }
    }

    private void DisplayYLabels(int width, int height)
    {
        if (displayedYLabels.Count != 11)
        {
            for (int i = 0; i < displayedYLabels.Count; i++)
            {
                statisticsAbsoluteLayout.Children.Remove(displayedYLabels[i]);
            }
            displayedYLabels.Clear();

            for (int y = 0; y < 11; y++)
            {
                int yCoord = GetYFromDataValue(y*10, width, height);

                Label label = new Label();
                label.Text = (y * 10).ToString();
                label.TextColor = TextColor;
                label.FontSize = YAxisTextFontSize;
                InitLabel(label, 0, height - yCoord, false);

                displayedYLabels.Add(label);

                statisticsAbsoluteLayout.Children.Add(label);
            }
        }
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

    private void DrawLine(int width, int height, Span<byte> data, Dictionary<DateOnly,int> entries)
    {
        if(entries is null)
            return;
        int lastXCoord = 0;
        int lastYCoord = 0;
        for (int i = 0; i < Math.Min(8, entries.Count); i++)
        {
            int xCoord = GetXFromDataValue(i - 1, width, height);
            int dataValue = entries.ElementAt(Math.Max(0, entries.Count - 8) + i).Value;
            int yCoord = GetYFromDataValue(dataValue, width, height);

            DrawDataPoint(xCoord, yCoord, width, height, data);

            if(i > 0)
            {
                ConnectDataPoints(width, height, xCoord, yCoord, lastXCoord, lastYCoord, data);
            }

            lastXCoord = xCoord;
            lastYCoord = yCoord;
        }
    }

    private void ConnectDataPoints(int width, int height, int xCoord, int yCoord, int lastXCoord, int lastYCoord, Span<byte> data)
    {
        Vector2 vec = Vector2.Normalize(new Vector2(xCoord - lastXCoord, yCoord - lastYCoord));
        float x = lastXCoord;
        float y = lastYCoord;

        while (x < xCoord)
        {
            x += vec.X;
            y += vec.Y;

            int index = GetIndexFromXY((int)x, (int)y, width, height);

            DataColor.ToRgb(out data[index + 2], out data[index + 1], out data[index + 0]);
        }
    }

    private static int GetYFromDataValue(int dataValue, int width, int height)
    {
        return (int)(charHeight + (dataValue / 10f) * (height - charHeight - arrowSteps) / (numYBars + 1f));
    }

    private int GetXFromDataValue(int dataValue, int width, int height)
    {
        int numXBars = graphDisplayMode == DisplayMode.Week ? numXBars_Week : numXBars_HalfYear;
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

    private static string DateToString(DateOnly date, DisplayMode curDisplayMode)
    {
        return curDisplayMode == DisplayMode.Week ? date.Day + "." + date.Month : date.ToString("MMM");
    }

    private async Task UpdateGraphDisplayMode(DisplayMode newMode)
    {
        if(newMode != graphDisplayMode)
        {
            graphDisplayMode = newMode;
            DisplayModeLabel.Text = graphDisplayMode == DisplayMode.Week ? "Woche" : "Halbjahr";
            await DrawChart();
        }
    }

    private async void RB_Week_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(e.Value == true)
        {
            await UpdateGraphDisplayMode(DisplayMode.Week);
        }
    }
    private async void RB_HalfYear_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value == true)
        {
            await UpdateGraphDisplayMode(DisplayMode.HalfYear);
        }
    }
}