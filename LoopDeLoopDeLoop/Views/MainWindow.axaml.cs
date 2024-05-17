using System;
using System.Collections.Generic;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using LoopDeLoopDeLoop.Components;

namespace LoopDeLoopDeLoop.Views;

public partial class MainWindow : Window
{
    private List<LoopFile> Drums;
    private List<LoopFile> Bass;
    private List<LoopFile> Guitar;
    private List<LoopFile> Piano;
    private List<LoopFile> Synth;
    private List<LoopFile> Custom;
    private List<LoopFile> Undefined;

    public AudioPlayer AudioPlayer;
    
    public MainWindow()
    {
        Drums = new List<LoopFile>();
        Bass = new List<LoopFile>();
        Guitar = new List<LoopFile>();
        Piano = new List<LoopFile>();
        Synth = new List<LoopFile>();
        Custom = new List<LoopFile>();
        Undefined = new List<LoopFile>();
        
        AudioPlayer = new AudioPlayer();
        
        InitializeComponent();
        // CreateButtons();
        CreateLists();
    }
    
    private void CreateButtons()
    {
        // TODO
    }

    private void CreateLists()
    {
        string audioFilePath = Path.Combine("Assets", "Loops");
        string[] allFiles = Directory.GetFiles(audioFilePath, "*.*", SearchOption.AllDirectories);

        foreach (string filePath in allFiles)
        {
            LoopFile newLoop = new LoopFile(filePath);
            switch (newLoop.GetCategory())
            {
                case "Drums":
                    Drums.Add(newLoop);
                    break;
                case "Bass":
                    Bass.Add(newLoop);
                    break;
                case "Guitar":
                    Guitar.Add(newLoop);
                    break;
                case "Piano":
                    Piano.Add(newLoop);
                    break;
                case "Synth":
                    Synth.Add(newLoop);
                    break;
                case "Custom":
                    Custom.Add(newLoop);
                    break;
                case "Undefined":
                    Undefined.Add(newLoop);
                    break;
            }
            // newLoop.OutputLoopInfo();
        }
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}