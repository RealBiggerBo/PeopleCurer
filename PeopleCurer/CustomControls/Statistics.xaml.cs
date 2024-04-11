using Microsoft.Maui.Controls.Shapes;
using System.IO;
using System.Numerics;
using System.Text;
using static System.Net.WebRequestMethods;

namespace PeopleCurer.CustomControls;

public partial class StatisticsControl : ContentView
{
    public static readonly BindableProperty DataProperty =
        BindableProperty.Create(nameof(Data), typeof(Dictionary<DateOnly, int>), typeof(StatisticsControl),
            propertyChanged: (bindable, oldVal, newVal) =>
            {
                StatisticsControl ctrl = (StatisticsControl)bindable;
                ctrl.dataPath.Data = ctrl.GetDataGeometry(newVal);
            });

    public static readonly BindableProperty YAxisTextFontSizeProperty =
        BindableProperty.Create(nameof(YAxisTextFontSize), typeof(double), typeof(StatisticsControl), 15d);

    public static readonly BindableProperty XAxisTextFontSizeProperty =
        BindableProperty.Create(nameof(XAxisTextFontSize), typeof(double), typeof(StatisticsControl), 15d);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(StatisticsControl));

    public static readonly BindableProperty AxisColorProperty =
        BindableProperty.Create(nameof(AxisColor), typeof(Color), typeof(StatisticsControl));

    public static readonly BindableProperty DataColorProperty =
        BindableProperty.Create(nameof(DataColor), typeof(Color), typeof(StatisticsControl));

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

	public StatisticsControl()
	{
		InitializeComponent();
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
        axisPath.Data = StringToGeometry(axisStringBuilder.ToString());

        //grid
        StringBuilder gridStringBuilder = new StringBuilder();
        GetGridString(gridStringBuilder);
        gridPath.Data = StringToGeometry(gridStringBuilder.ToString());

        //data
        dataPath.Data = GetDataGeometry(Data);

        //text
        Data ??= [];

        if(Data.Count > 0)
            labelX1.Text = DateToString(Data.ElementAt(0).Key);
        if (Data.Count > 1)
            labelX2.Text = DateToString(Data.ElementAt(1).Key);
        if (Data.Count > 2)
            labelX3.Text = DateToString(Data.ElementAt(2).Key);
        if (Data.Count > 3)
            labelX4.Text = DateToString(Data.ElementAt(3).Key);
        if (Data.Count > 4)
            labelX5.Text = DateToString(Data.ElementAt(4).Key);
        if (Data.Count > 5)
            labelX6.Text = DateToString(Data.ElementAt(5).Key);
        if (Data.Count > 6)
            labelX7.Text = DateToString(Data.ElementAt(6).Key);

        //Update UI
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

    private void StatisticsAbsoluteLayout_SizeChanged(object sender, EventArgs e)
    {
        if (sender is AbsoluteLayout layout)
            RedrawControl((float)layout.Width, (float)layout.Height);
    }

    private void Label0_SizeChanged(object sender, EventArgs e)
    {
        label0.TranslationX = startingPoint.X - label0.Width - barDimensions.X;
        label0.TranslationY = startingPoint.Y - (0) * barOffsets.Y - label0.Height / 2;
    }
    private void Label10_SizeChanged(object sender, EventArgs e)
    {
        label10.TranslationX = startingPoint.X - label10.Width - barDimensions.X;
        label10.TranslationY = startingPoint.Y - (1) * barOffsets.Y - label10.Height / 2;
    }
    private void Label20_SizeChanged(object sender, EventArgs e)
    {
        label20.TranslationX = startingPoint.X - label20.Width - barDimensions.X;
        label20.TranslationY = startingPoint.Y - (2) * barOffsets.Y - label20.Height / 2;
    }
    private void Label30_SizeChanged(object sender, EventArgs e)
    {
        label30.TranslationX = startingPoint.X - label30.Width - barDimensions.X;
        label30.TranslationY = startingPoint.Y - (3) * barOffsets.Y - label30.Height / 2;
    }
    private void Label40_SizeChanged(object sender, EventArgs e)
    {
        label40.TranslationX = startingPoint.X - label40.Width - barDimensions.X;
        label40.TranslationY = startingPoint.Y - (4) * barOffsets.Y - label40.Height / 2;
    }
    private void Label50_SizeChanged(object sender, EventArgs e)
    {
        label50.TranslationX = startingPoint.X - label50.Width - barDimensions.X;
        label50.TranslationY = startingPoint.Y - (5) * barOffsets.Y - label50.Height / 2;
    }
    private void Label60_SizeChanged(object sender, EventArgs e)
    {
        label60.TranslationX = startingPoint.X - label60.Width - barDimensions.X;
        label60.TranslationY = startingPoint.Y - (6) * barOffsets.Y - label60.Height / 2;
    }
    private void Label70_SizeChanged(object sender, EventArgs e)
    {
        label70.TranslationX = startingPoint.X - label70.Width - barDimensions.X;
        label70.TranslationY = startingPoint.Y - (7) * barOffsets.Y - label70.Height / 2;
    }
    private void Label80_SizeChanged(object sender, EventArgs e)
    {
        label80.TranslationX = startingPoint.X - label80.Width - barDimensions.X;
        label80.TranslationY = startingPoint.Y - (8) * barOffsets.Y - label80.Height / 2;
    }
    private void Label90_SizeChanged(object sender, EventArgs e)
    {
        label90.TranslationX = startingPoint.X - label90.Width - barDimensions.X;
        label90.TranslationY = startingPoint.Y - (9) * barOffsets.Y - label90.Height / 2;
    }
    private void Label100_SizeChanged(object sender, EventArgs e)
    {
        label100.TranslationX = startingPoint.X - label100.Width - barDimensions.X;
        label100.TranslationY = startingPoint.Y - (10) * barOffsets.Y - label100.Height / 2;
    }

    private void LabelX1_SizeChanged(object sender, EventArgs e)
    {
        labelX1.TranslationX = startingPoint.X - labelX1.Width / 2 + 1*barOffsets.X;
        labelX1.TranslationY = startingPoint.Y + barDimensions.Y;
    }
    private void LabelX2_SizeChanged(object sender, EventArgs e)
    {
        labelX2.TranslationX = startingPoint.X - labelX2.Width / 2 + 2 * barOffsets.X;
        labelX2.TranslationY = startingPoint.Y + barDimensions.Y;
    }
    private void LabelX3_SizeChanged(object sender, EventArgs e)
    {
        labelX3.TranslationX = startingPoint.X - labelX3.Width / 2 + 3 * barOffsets.X;
        labelX3.TranslationY = startingPoint.Y + barDimensions.Y;
    }
    private void LabelX4_SizeChanged(object sender, EventArgs e)
    {
        labelX4.TranslationX = startingPoint.X - labelX4.Width / 2 + 4 * barOffsets.X;
        labelX4.TranslationY = startingPoint.Y + barDimensions.Y;
    }
    private void LabelX5_SizeChanged(object sender, EventArgs e)
    {
        labelX5.TranslationX = startingPoint.X - labelX5.Width / 2 + 5 * barOffsets.X;
        labelX5.TranslationY = startingPoint.Y + barDimensions.Y;
    }
    private void LabelX6_SizeChanged(object sender, EventArgs e)
    {
        labelX6.TranslationX = startingPoint.X - labelX6.Width / 2 + 6 * barOffsets.X;
        labelX6.TranslationY = startingPoint.Y + barDimensions.Y;
    }
    private void LabelX7_SizeChanged(object sender, EventArgs e)
    {
        labelX7.TranslationX = startingPoint.X - labelX7.Width / 2 + 7 * barOffsets.X;
        labelX7.TranslationY = startingPoint.Y + barDimensions.Y;
    }
}