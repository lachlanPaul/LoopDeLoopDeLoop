using System;
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
    private List<Button> _drumsButtons;
    
    private List<LoopFile> _bass;
    private List<Button> _bassButtons;
    
    private List<LoopFile> _guitar;
    private List<Button> _guitarButtons;
    
    private List<LoopFile> _piano;
    private List<Button> _pianoButtons;
    
    private List<LoopFile> _synth;
    private List<Button> _synthButtons;
    
    private List<LoopFile> _custom;
    private List<Button> _customButtons;
    
    private List<LoopFile> _undefined;
    private List<Button> _undefinedButtons;

    private List<Button> CurrentCategory;
    
    public AudioPlayer AudioPlayer;
    
    
    public MainWindow()
    {
        _drums = new List<LoopFile>();
        _drumsButtons = new List<Button>();
        
        _bass = new List<LoopFile>();
        _bassButtons = new List<Button>();
        
        _guitar = new List<LoopFile>();
        _guitarButtons = new List<Button>();
        
        _piano = new List<LoopFile>();
        _pianoButtons = new List<Button>();
        
        _synth = new List<LoopFile>();
        _synthButtons = new List<Button>();
        
        _custom = new List<LoopFile>();
        _customButtons = new List<Button>();
        
        _undefined = new List<LoopFile>();
        _undefinedButtons = new List<Button>();
        
        AudioPlayer = new AudioPlayer();
        
        InitializeComponent();
        CreateLoops();
        DisplayButtons(_drumsButtons);
        
        // var newLoopMenuItem = this.FindControl<MenuItem>("NewLoopMenuItem");
        // newLoopMenuItem.Click += NewLoopMenuItem_Click;
    }
    
    private Button CreateButton(LoopFile loop)
    {
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

        return loopButton;
    }

    /// <summary>
    /// Creates loops from all the audio files, adds them to the right lists, and creates a button.
    /// </summary>
    private void CreateLoops()
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
                    _drumsButtons.Add(CreateButton(newLoop));
                    break;
                case "Bass":
                    _bass.Add(newLoop);
                    _bassButtons.Add(CreateButton(newLoop));
                    break;
                case "Guitar":
                    _guitar.Add(newLoop);
                    _guitarButtons.Add(CreateButton(newLoop));
                    break;
                case "Piano":
                    _piano.Add(newLoop);
                    _pianoButtons.Add(CreateButton(newLoop));
                    break;
                case "Synth":
                    _synth.Add(newLoop);
                    _synthButtons.Add(CreateButton(newLoop));
                    break;
                case "Custom":
                    _custom.Add(newLoop);
                    _customButtons.Add(CreateButton(newLoop));
                    break;
                case "Undefined":
                    _undefined.Add(newLoop);
                    _undefinedButtons.Add(CreateButton(newLoop));
                    break;
            }
            // newLoop.OutputLoopInfo();
        }
        Console.WriteLine("All Loops Processed");
    }

    /// <summary>
    /// Displays new list of buttons based on category selected
    /// </summary>
    /// <param name="list">List of buttons to use</param>
    private void DisplayButtons(List<Button> list)
    {
        var buttonsContainer = this.FindControl<ItemsControl>("ButtonsContainer");
        
        if (list != CurrentCategory)
        {
            buttonsContainer.Items.Clear();
            
            foreach (var button in list)
            {
                buttonsContainer.Items.Add(button);
            }

            CurrentCategory = list;

            Console.WriteLine("All Buttons Displayed");
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
            var customDir = @"Assets\Loops\Custom\";

            if (!Directory.Exists(customDir))
            {
                Directory.CreateDirectory(customDir);
            }

            string sourceFilePath = result[0];
            string fileName = Path.GetFileName(sourceFilePath);
            string destinationFilePath = Path.Combine(customDir, fileName);

            File.Copy(sourceFilePath, destinationFilePath, true);

            LoopFile newLoop = new LoopFile(destinationFilePath);
            CreateButton(newLoop);
        }
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}