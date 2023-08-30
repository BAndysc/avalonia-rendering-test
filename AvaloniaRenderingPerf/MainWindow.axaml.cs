using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Rendering;
using Avalonia.VisualTree;

namespace AvaloniaRenderingPerf;

public partial class MainWindow : Window
{
    public class ThisDataContext : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private string resolution = "";
        public string Resolution
        {
            get => resolution;
            set => SetField(ref resolution, value);
        }
    }

    private ThisDataContext context;
    
    public MainWindow()
    {
        InitializeComponent();
        context = new ThisDataContext();
        DataContext = context;
        var size = GetPixelSize(this.GetVisualRoot());
        context.Resolution = $"{size.Width}x{size.Height}";
    }

    protected override void OnResized(WindowResizedEventArgs e)
    {
        base.OnResized(e);
        var size = GetPixelSize(this.GetVisualRoot());
        context.Resolution = $"{size.Width}x{size.Height}";
    }

    private PixelSize GetPixelSize(IRenderRoot visualRoot)
    {
        var scaling = visualRoot.RenderScaling;
        return new PixelSize(Math.Max(1, (int)(Bounds.Width * scaling)),
            Math.Max(1, (int)(Bounds.Height * scaling)));
    }
}