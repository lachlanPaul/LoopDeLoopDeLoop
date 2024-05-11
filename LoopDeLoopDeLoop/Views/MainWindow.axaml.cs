using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LoopDeLoopDeLoop.Components;

namespace LoopDeLoopDeLoop.Views;

public partial class MainWindow : Window
{
    public List<LoopFile> Drums;
    public List<LoopFile> Guitar;
    public List<LoopFile> Piano;
    public List<LoopFile> Synth;
    
    public MainWindow()
    {
        InitializeComponent();
        CreateButtons();
    }
    
    private void CreateButtons()
    {
        var items = new List<string> { "Item1", "Item2", "Item3" }; // Your array here
        var buttonsContainer = this.FindControl<ItemsControl>("ButtonsContainer");

        foreach (var item in items)
        {
            var button = new Button
            {
                Content = item,
                Margin = new Thickness(2)
            };
            // Optionally, handle the Click event of the button
            button.Click += (sender, e) =>
            {
                // Handle button click
            };
            buttonsContainer.Items.Add(button);
        }
    }

    public void CreateLists()
    {
        
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}