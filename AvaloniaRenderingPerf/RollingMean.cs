namespace AvaloniaRenderingPerf;

public struct RollingMean
{
    private double[] values;
    private double sum;
    private int index;
    private int count;

    public RollingMean()
    {
        values = new double[32];
        sum = 0;
        index = 0;
        count = 1;
    }

    public void Add(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
            return;

        sum -= values[index];
        values[index] = value;
        sum += value;
        index = (index + 1) % values.Length;
        if (count < values.Length)
            count++;
    }

    public double Value => sum / count;
}