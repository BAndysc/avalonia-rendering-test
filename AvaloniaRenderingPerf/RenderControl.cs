using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;

namespace AvaloniaRenderingPerf;

public class RenderControl : Control
{
    private Stopwatch sw = new Stopwatch();
    
    private RollingMean fps = new RollingMean();

    public RenderControl()
    {
        sw.Start();
    }

    public override void Render(DrawingContext context)
    {
        Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
        
        var elapsed = sw.ElapsedMilliseconds;
        fps.Add(1000.0 / elapsed);
        Console.WriteLine(fps.Value);
        sw.Restart();
        
        base.Render(context);
        context.FillRectangle(Brushes.Red, new Rect(0, 0, Bounds.Width, Bounds.Width));
    }
}