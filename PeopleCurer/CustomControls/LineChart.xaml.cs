using Microsoft.Maui.Controls.Shapes;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace PeopleCurer.CustomControls;

public partial class LineChart : ContentView
{
    public static readonly BindableProperty DataProperty =
        BindableProperty.Create(nameof(Data), typeof(Dictionary<DateOnly, int>), typeof(LineChart),
            propertyChanged: async (bindable, oldVal, newVal) =>
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

    private int RenderWith = 200;
    private int RenderHeight = 200;

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
        if (Data is null || RenderWith == 0 || RenderHeight == 0)
            return;

        //create bitmap
        byte[] buffer = GetData(RenderWith, RenderHeight, Data);

        //update text
        //while (stackLayout.Children.Count > 1)
        //{
        //    stackLayout.Children.RemoveAt(1);
        //}

        //for (int i = 0; i < Data.Count; i++)
        //{
        //    HorizontalStackLayout horizontalLayout = new HorizontalStackLayout();
        //    horizontalLayout.VerticalOptions = LayoutOptions.Center;
        //    horizontalLayout.Spacing = 5;

        //    Rectangle rect = new Rectangle();
        //    rect.BackgroundColor = Color.FromHsv((float)i / Data.Count, 1, 1);
        //    rect.WidthRequest = 20;
        //    rect.HeightRequest = 20;
        //    horizontalLayout.Children.Add(rect);

        //    Label label = new Label();
        //    label.Text = Data[i].thoughtName;
        //    horizontalLayout.Children.Add(label);

        //    stackLayout.Children.Add(horizontalLayout);
        //}

        //show image
        chartImage.Source = ImageSource.FromStream(() => new MemoryStream(buffer));
    }

    private static byte[] GetData(int width, int height, Dictionary<DateOnly, int> data)
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
        int ySpacing = (height - arrowSteps - charHeight - 10) / 10;
        int xSpacing = (width - arrowSteps - charWidth * 3 - 7) / 7;

        int numXBars = 0;
        int numYBars = 0;

        int index = 54;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 uv = new Vector2(x, y);

                byte r = 0xFF;
                byte g = 0x00;
                byte b = 0xFF;

                Vector2 vec = uv - new Vector2(width / 2, height / 2);

                if ((x == charWidth*3 && y >=charHeight) || (y == charHeight && x >= charWidth*3))
                {
                    //on axis
                    r = 0xFF; g = 0xFF; b = 0xFF;
                }
                else if((y >= height - arrowSteps) && (MathF.Abs(x-charWidth*3) <= height - y))
                {
                    //on yAxis arrow
                    r = 0xFF; g= 0x00; b = 0x00;
                }
                else if ((x >= width - arrowSteps) && (MathF.Abs(y - charHeight) <= width - x))
                {
                    //on xAxis arrow
                    r = 0x00; g = 0xFF; b = 0x00;
                }
                else if(x>charWidth*3 && (x-charWidth*3)%xSpacing == 0 && MathF.Abs(y - charHeight) <= arrowSteps/2 && x < width - arrowSteps)
                {
                    //on xAxis bar
                    r = 0x00; g = 0x00; b = 0xFF;
                }
                else if(y>charHeight && (y-charHeight)%ySpacing == 0 && MathF.Abs(x-charWidth*3) <= arrowSteps/2 &&  y < height - arrowSteps - 1)
                {
                    //on yAxis bar
                    r = 0x00; g = 0x00; b = 0x00;
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

        DrawArrows(width, height, buffer.AsSpan<byte>());

        return buffer;
    }

    private static void DrawArrows(int width, int height, Span<byte> data)
    {
        int stepCount = 10;
        for (int i = 0; i < stepCount; i++)
        {
            int y = height - i;
        }
    }

    private static void IntToBytes(int integer, out byte a, out byte b, out byte c, out byte d)
    {
        a = (byte)(integer & 0x000000ff);
        b = (byte)((integer & 0x0000ff00) >> 8);
        c = (byte)((integer & 0x00ff0000) >> 16);
        d = (byte)((integer & 0xff000000) >> 24);
    }

    private void RedrawControl(float width, float height)
    {
        float yAxisLength = height - textSpace.Y;
        float xAxisLength = width - textSpace.X;
        axisLength = new Vector2(xAxisLength, yAxisLength);
        startingPoint = new Vector2(width - xAxisLength, yAxisLength);

        //axis
        StringBuilder axisStringBuilder = new StringBuilder();
        GetXAxisString(axisStringBuilder, axisLength.X);
        GetYAxisString(axisStringBuilder, axisLength.Y);
        //axisPath.Data = StringToGeometry(axisStringBuilder.ToString());

        //grid
        StringBuilder gridStringBuilder = new StringBuilder();
        GetGridString(gridStringBuilder);
        //gridPath.Data = StringToGeometry(gridStringBuilder.ToString());

        //data
        //dataPath.Data = GetDataGeometry(Data);

        //text
        Data ??= [];

        //if(Data.Count > 0)
        //    labelX1.Text = DateToString(Data.ElementAt(0).Key);
        //if (Data.Count > 1)
        //    labelX2.Text = DateToString(Data.ElementAt(1).Key);
        //if (Data.Count > 2)
        //    labelX3.Text = DateToString(Data.ElementAt(2).Key);
        //if (Data.Count > 3)
        //    labelX4.Text = DateToString(Data.ElementAt(3).Key);
        //if (Data.Count > 4)
        //    labelX5.Text = DateToString(Data.ElementAt(4).Key);
        //if (Data.Count > 5)
        //    labelX6.Text = DateToString(Data.ElementAt(5).Key);
        //if (Data.Count > 6)
        //    labelX7.Text = DateToString(Data.ElementAt(6).Key);


        //Label0_SizeChanged(null,EventArgs.Empty);
        //Label10_SizeChanged(null, EventArgs.Empty);
        //Label20_SizeChanged(null, EventArgs.Empty);
        //Label30_SizeChanged(null, EventArgs.Empty);
        //Label40_SizeChanged(null, EventArgs.Empty);
        //Label50_SizeChanged(null, EventArgs.Empty);
        //Label60_SizeChanged(null, EventArgs.Empty);
        //Label70_SizeChanged(null, EventArgs.Empty);
        //Label80_SizeChanged(null, EventArgs.Empty);
        //Label90_SizeChanged(null, EventArgs.Empty);
        //Label100_SizeChanged(null, EventArgs.Empty);

        //LabelX1_SizeChanged(null, EventArgs.Empty);
        //LabelX2_SizeChanged(null, EventArgs.Empty);
        //LabelX3_SizeChanged(null, EventArgs.Empty);
        //LabelX4_SizeChanged(null, EventArgs.Empty);
        //LabelX5_SizeChanged(null, EventArgs.Empty);
        //LabelX6_SizeChanged(null, EventArgs.Empty);
        //LabelX7_SizeChanged(null, EventArgs.Empty);

        //UpdateCanGoToLesson UI
        base.Dispatcher.Dispatch(() => base.InvalidateMeasure());
    }

    private void GetGridString(StringBuilder s)
    {
        for (int i = 0; i < barCounts.X; i++)
        {
            s.Append('M');
            s.Append(startingPoint.X + (i + 1) * barOffsets.X);
            s.Append(',');
            s.Append(startingPoint.Y);
            s.Append('V');
            s.Append(startingPoint.Y - barOffsets.Y * barCounts.Y - 0.5f * barOffsets.Y);
        }
        for (int i = 0; i < barCounts.Y; i++)
        {
            s.Append('M');
            s.Append(startingPoint.X);
            s.Append(',');
            s.Append(startingPoint.Y - (i + 1) * barOffsets.Y);
            s.Append('H');
            s.Append(startingPoint.X + barOffsets.X * barCounts.X + 0.5f * barOffsets.X);
        }
    }

    private static PathGeometry StringToGeometry(string path)
    {
        PathFigureCollection figureCollection = new PathFigureCollection();
        PathFigureCollectionConverter.ParseStringToPathFigureCollection(figureCollection, path);
        return new PathGeometry(figureCollection);
    }

    private static string DateToString(DateOnly date)
    {
        return date.Day + "." + date.Month;
    }

    private void GetYAxisString(StringBuilder s, float yAxisLength)
    {
        s.Append('M');
        s.Append(startingPoint.X);
        s.Append(' ');
        s.Append(startingPoint.Y);
        s.Append('V');
        s.Append(0);
        //Arrow
        float arrowOffsetX = 3;
        float arrowOffsetY = 3;
        s.Append('L');
        s.Append(startingPoint.X - arrowOffsetX);
        s.Append(',');
        s.Append(arrowOffsetY);
        s.Append('L');
        s.Append(startingPoint.X + arrowOffsetX);
        s.Append(',');
        s.Append(arrowOffsetY);
        s.Append('L');
        s.Append(startingPoint.X);
        s.Append(',');
        s.Append(0);
        //bars
        barOffsets.Y = yAxisLength / (barCounts.Y + 1);
        for (int i = 0; i < barCounts.Y; i++)
        {
            s.Append('M');
            s.Append(startingPoint.X - barDimensions.X);
            s.Append(',');
            s.Append(startingPoint.Y - (i + 1) * barOffsets.Y);
            s.Append('H');
            s.Append(startingPoint.X + barDimensions.X);
        }
    }

    private void GetXAxisString(StringBuilder s, float xAxisLength)
    {
        s.Append('M');
        s.Append(startingPoint.X);
        s.Append(' ');
        s.Append(startingPoint.Y);
        s.Append('H');
        s.Append(startingPoint.X + xAxisLength);
        //Arrow
        float arrowOffsetX = 3;
        float arrowOffsetY = 3;
        s.Append('L');
        s.Append(startingPoint.X + xAxisLength - arrowOffsetX);
        s.Append(',');
        s.Append(startingPoint.Y - arrowOffsetY);
        s.Append('L');
        s.Append(startingPoint.X + xAxisLength - arrowOffsetX);
        s.Append(',');
        s.Append(startingPoint.Y + arrowOffsetY);
        s.Append('L');
        s.Append(startingPoint.X + xAxisLength);
        s.Append(',');
        s.Append(startingPoint.Y);
        //bars
        barOffsets.X = xAxisLength / (barCounts.X + 1);
        for (int i = 0; i < barCounts.X; i++)
        {
            s.Append('M');
            s.Append(startingPoint.X + (i+1) * barOffsets.X);
            s.Append(',');
            s.Append(startingPoint.Y - barDimensions.Y);
            s.Append('V');
            s.Append(startingPoint.Y + barDimensions.Y);
        }
    }

    private PathGeometry? GetDataGeometry(object value)
    {
        if (value is Dictionary<DateOnly, int> data)
        {
            return StringToGeometry(GetDataString(data));
        }
        return null;
    }

    private string GetDataString(Dictionary<DateOnly, int> data)
    {
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < data.Count; i++)
        {
            if (i == 0)
                builder.Append('M');
            else
                builder.Append('L');
            builder.Append((i + 1) * barOffsets.X + startingPoint.X);
            builder.Append(',');
            builder.Append(startingPoint.Y - data.ElementAt(i).Value * barOffsets.Y / 10f);
        }

        return builder.ToString();
    }

    private async void StatisticsAbsoluteLayout_SizeChanged(object sender, EventArgs e)
    {
        if (sender is AbsoluteLayout layout && layout.Width != 0 && layout.Height != 0)
        {
            await Task.Run(() => RedrawControl((float)layout.Width, (float)layout.Height));
        }
    }

    //private void Label0_SizeChanged(object sender, EventArgs e)
    //{
    //    label0.TranslationX = startingPoint.X - label0.Width - barDimensions.X;
    //    label0.TranslationY = startingPoint.Y - (0) * barOffsets.Y - label0.Height / 2;
    //}
    //private void Label10_SizeChanged(object sender, EventArgs e)
    //{
    //    label10.TranslationX = startingPoint.X - label10.Width - barDimensions.X;
    //    label10.TranslationY = startingPoint.Y - (1) * barOffsets.Y - label10.Height / 2;
    //}
    //private void Label20_SizeChanged(object sender, EventArgs e)
    //{
    //    label20.TranslationX = startingPoint.X - label20.Width - barDimensions.X;
    //    label20.TranslationY = startingPoint.Y - (2) * barOffsets.Y - label20.Height / 2;
    //}
    //private void Label30_SizeChanged(object sender, EventArgs e)
    //{
    //    label30.TranslationX = startingPoint.X - label30.Width - barDimensions.X;
    //    label30.TranslationY = startingPoint.Y - (3) * barOffsets.Y - label30.Height / 2;
    //}
    //private void Label40_SizeChanged(object sender, EventArgs e)
    //{
    //    label40.TranslationX = startingPoint.X - label40.Width - barDimensions.X;
    //    label40.TranslationY = startingPoint.Y - (4) * barOffsets.Y - label40.Height / 2;
    //}
    //private void Label50_SizeChanged(object sender, EventArgs e)
    //{
    //    label50.TranslationX = startingPoint.X - label50.Width - barDimensions.X;
    //    label50.TranslationY = startingPoint.Y - (5) * barOffsets.Y - label50.Height / 2;
    //}
    //private void Label60_SizeChanged(object sender, EventArgs e)
    //{
    //    label60.TranslationX = startingPoint.X - label60.Width - barDimensions.X;
    //    label60.TranslationY = startingPoint.Y - (6) * barOffsets.Y - label60.Height / 2;
    //}
    //private void Label70_SizeChanged(object sender, EventArgs e)
    //{
    //    label70.TranslationX = startingPoint.X - label70.Width - barDimensions.X;
    //    label70.TranslationY = startingPoint.Y - (7) * barOffsets.Y - label70.Height / 2;
    //}
    //private void Label80_SizeChanged(object sender, EventArgs e)
    //{
    //    label80.TranslationX = startingPoint.X - label80.Width - barDimensions.X;
    //    label80.TranslationY = startingPoint.Y - (8) * barOffsets.Y - label80.Height / 2;
    //}
    //private void Label90_SizeChanged(object sender, EventArgs e)
    //{
    //    label90.TranslationX = startingPoint.X - label90.Width - barDimensions.X;
    //    label90.TranslationY = startingPoint.Y - (9) * barOffsets.Y - label90.Height / 2;
    //}
    //private void Label100_SizeChanged(object sender, EventArgs e)
    //{
    //    label100.TranslationX = startingPoint.X - label100.Width - barDimensions.X;
    //    label100.TranslationY = startingPoint.Y - (10) * barOffsets.Y - label100.Height / 2;
    //}

    //private void LabelX1_SizeChanged(object sender, EventArgs e)
    //{
    //    labelX1.TranslationX = startingPoint.X - labelX1.Width / 2 + 1*barOffsets.X;
    //    labelX1.TranslationY = startingPoint.Y + barDimensions.Y;
    //}
    //private void LabelX2_SizeChanged(object sender, EventArgs e)
    //{
    //    labelX2.TranslationX = startingPoint.X - labelX2.Width / 2 + 2 * barOffsets.X;
    //    labelX2.TranslationY = startingPoint.Y + barDimensions.Y;
    //}
    //private void LabelX3_SizeChanged(object sender, EventArgs e)
    //{
    //    labelX3.TranslationX = startingPoint.X - labelX3.Width / 2 + 3 * barOffsets.X;
    //    labelX3.TranslationY = startingPoint.Y + barDimensions.Y;
    //}
    //private void LabelX4_SizeChanged(object sender, EventArgs e)
    //{
    //    labelX4.TranslationX = startingPoint.X - labelX4.Width / 2 + 4 * barOffsets.X;
    //    labelX4.TranslationY = startingPoint.Y + barDimensions.Y;
    //}
    //private void LabelX5_SizeChanged(object sender, EventArgs e)
    //{
    //    labelX5.TranslationX = startingPoint.X - labelX5.Width / 2 + 5 * barOffsets.X;
    //    labelX5.TranslationY = startingPoint.Y + barDimensions.Y;
    //}
    //private void LabelX6_SizeChanged(object sender, EventArgs e)
    //{
    //    labelX6.TranslationX = startingPoint.X - labelX6.Width / 2 + 6 * barOffsets.X;
    //    labelX6.TranslationY = startingPoint.Y + barDimensions.Y;
    //}
    //private void LabelX7_SizeChanged(object sender, EventArgs e)
    //{
    //    labelX7.TranslationX = startingPoint.X - labelX7.Width / 2 + 7 * barOffsets.X;
    //    labelX7.TranslationY = startingPoint.Y + barDimensions.Y;
    //}
}