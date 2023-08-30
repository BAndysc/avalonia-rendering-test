using System;
using System.Diagnostics;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;

namespace AvaloniaRenderingPerf;

public class OpenGlView : OpenGlControlBase
{
    public const int GL_COLOR_BUFFER_BIT = 0x00004000;
    public const int GL_DEPTH_BUFFER_BIT = 0x00000100;

    private Stopwatch sw = new Stopwatch();
    
    private RollingMean fps = new RollingMean();

    public OpenGlView()
    {
        sw.Start();
    }
    
    protected override void OnOpenGlRender(GlInterface gl, int fb)
    {
        var elapsed = sw.ElapsedMilliseconds;
        fps.Add(1000.0 / elapsed);
        Console.WriteLine(fps.Value);
        sw.Restart();
        Dispatcher.UIThread.Post(RequestNextFrameRendering, DispatcherPriority.Render);
        gl.ClearColor(1, 1, 0,1);
        gl.Clear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    }
}