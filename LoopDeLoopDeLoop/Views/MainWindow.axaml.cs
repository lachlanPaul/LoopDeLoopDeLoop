using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using LoopDeLoopDeLoop.Components;

namespace LoopDeLoopDeLoop.Views;

public partial class MainWindow : Window
{
    private List<LoopFile> _drums;
    private List<LoopFile> _bass;
    private List<LoopFile> _guitar;
    private List<LoopFile> _piano;
    private List<LoopFile> _synth;
    private List<LoopFile> _custom;
    private List<LoopFile> _undefined;

    public AudioPlayer AudioPlayer;
    
    public MainWindow()
    {
        _drums = new List<LoopFile>();
        _bass = new List<LoopFile>();
        _guitar = new List<LoopFile>();
        _piano = new List<LoopFile>();
        _synth = new List<LoopFile>();
        _custom = new List<LoopFile>();
        _undefined = new List<LoopFile>();
        
        AudioPlayer = new AudioPlayer();
        
        InitializeComponent();
        CreateLists();
        CreateButton(_drums[0]);
        
        var newLoopMenuItem = this.FindControl<MenuItem>("NewLoopMenuItem");
        newLoopMenuItem.Click += NewLoopMenuItem_Click;
    }
    
    private void CreateButton(LoopFile loop)
    {
        var buttonsContainer = this.FindControl<ItemsControl>("ButtonsContainer");

        var loopButton = new Button
        {
            Content = loop.GetFileName(),
            Width = 160,
            Height = 40
        };

        loopButton.Click += (sender, e) => 
        {
            AudioPlayer.Play(loop);
        };

        buttonsContainer.Items.Add(loopButton);
    }

    private void CreateLists()
    {
        string audioFilePath = Path.Combine("Assets", "Loops");
        if (!Directory.Exists(audioFilePath))
        {
            Directory.CreateDirectory(audioFilePath);
            System.Diagnostics.Debug.WriteLine("Loops folder could not be found, making a new one now.\n" +
                                               "To populate with the default loops, it's recommended you re-download the app, or make an issue on Github.");
        }
        string[] allFiles = Directory.GetFiles(audioFilePath, "*.*", SearchOption.AllDirectories);

        foreach (string filePath in allFiles)
        {
            LoopFile newLoop = new LoopFile(filePath);
            switch (newLoop.GetCategory())
            {
                case "Drums":
                    _drums.Add(newLoop);
                    break;
                case "Bass":
                    _bass.Add(newLoop);
                    break;
                case "Guitar":
                    _guitar.Add(newLoop);
                    break;
                case "Piano":
                    _piano.Add(newLoop);
                    break;
                case "Synth":
                    _synth.Add(newLoop);
                    break;
                case "Custom":
                    _custom.Add(newLoop);
                    break;
                case "Undefined":
                    _undefined.Add(newLoop);
                    break;
            }
            // newLoop.OutputLoopInfo();
        }
    }

    private async void NewLoopMenuItem_Click(object? sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();
        dialog.Title = "Select File for Custom Loop";
        dialog.Filters.Add(new FileDialogFilter {Extensions = new List<string> {"wav", "mp3", "flac"}});

        string[] result = await dialog.ShowAsync(this);

        if (result != null && result.Length > 0)
        {
            FileInfo file = new FileInfo(result[0]);
            file = file.CopyTo(@"Assets\Loops\Custom\");
            LoopFile newLoop = new LoopFile(file.DirectoryName);
            CreateButton(newLoop);
        }
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}