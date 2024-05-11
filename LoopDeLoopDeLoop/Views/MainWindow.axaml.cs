using System;
using System.Collections.Generic;
using System.IO;
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
        // TODO
    }

    public void CreateLists()
    {
        string[] allFiles;
        
        if (Directory.Exists("./Assets/Loops"))
        {
            allFiles = Directory.GetFiles("Assets/Loops", "*", SearchOption.AllDirectories);
            Console.WriteLine(allFiles);
        }
        else
        {
            throw new DirectoryNotFoundException("Folder that stores the loops could not be found (Assets/Loops). Check it exists or reinstall program");
        }
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}