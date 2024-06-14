using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
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

    private List<LoopFile> col1Loops;
    private List<LoopFile> col2Loops;
    private List<LoopFile> col3Loops;
    private List<LoopFile> col4Loops;
    
    private ItemsControl CurrentColumn;
    
    public AudioPlayer audioPlayer;


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

        audioPlayer = new AudioPlayer();

        InitializeComponent();
        CreateSongColumns();
        CreateLoops();
        CreateMediaControl();

        col1Loops = new List<LoopFile>();
        col2Loops = new List<LoopFile>();
        col3Loops = new List<LoopFile>();
        col4Loops = new List<LoopFile>();
        
        List<Button> categoryButtons = new List<Button> { 
            CreateCategoryButton("Drums", _drumsButtons),
            CreateCategoryButton("Bass", _bassButtons),
            CreateCategoryButton("Guitar", _guitarButtons),
            CreateCategoryButton("Piano", _pianoButtons),
            CreateCategoryButton("Synth", _synthButtons),
            CreateCategoryButton("Custom", _customButtons),
            CreateCategoryButton("Undfnd", _undefinedButtons)
        };

        var categoryContainer = this.FindControl<ItemsControl>("CategoryControl");
        
        foreach (Button button in categoryButtons)
        {
            categoryContainer.Items.Add(button);
        }

        DisplayButtons(_drumsButtons);
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
                    _drumsButtons.Add(CreateLoopButton(newLoop));
                    break;
                case "Bass":
                    _bass.Add(newLoop);
                    _bassButtons.Add(CreateLoopButton(newLoop));
                    break;
                case "Guitar":
                    _guitar.Add(newLoop);
                    _guitarButtons.Add(CreateLoopButton(newLoop));
                    break;
                case "Piano":
                    _piano.Add(newLoop);
                    _pianoButtons.Add(CreateLoopButton(newLoop));
                    break;
                case "Synth":
                    _synth.Add(newLoop);
                    _synthButtons.Add(CreateLoopButton(newLoop));
                    break;
                case "Custom":
                    _custom.Add(newLoop);
                    _customButtons.Add(CreateLoopButton(newLoop));
                    break;
                case "Undefined":
                    _undefined.Add(newLoop);
                    _undefinedButtons.Add(CreateLoopButton(newLoop));
                    break;
            }
            newLoop.OutputLoopInfo();
        }
        Console.WriteLine("All Loops Processed");
    }

    private void CreateSongColumns()
    {
        for (int i = 2; i != 6; i++)
        {
            // This may be wasting power re-initilising these every loop, but they're only used here, and it's not much so idk
            var grid = this.FindControl<Grid>("Grid");
            
            List<IBrush> colorList = new List<IBrush>
            {
                Brushes.Red,
                Brushes.Blue,
                Brushes.Green,
                Brushes.Yellow
            };
            
            var itemColumn = new ItemsControl
            {
                Name = "LoopCol" + $"{i - 1}",
                Background = colorList[i - 2]  // TODO: Decide colour
            };

            itemColumn.PointerReleased += (sender, e) =>
            {
                CurrentColumn = itemColumn;
                Console.WriteLine(CurrentColumn);
            };
            
            Grid.SetColumn(itemColumn, i);
            Grid.SetRow(itemColumn, 2);
            
            grid.Children.Add(itemColumn);
        }
    }
    
    private Button CreateLoopButton(LoopFile loop, bool isSongColumn = false)
    {
        List<LoopFile> buttonCol = new List<LoopFile>();
        
        var loopButton = new Button
        {
            Content = loop.GetFileName(),
            Width = 160,
            Height = 40,
        };

        loopButton.Click += (sender, e) =>
        {
            audioPlayer.Play(loop);
        };

        var contextMenu = new ContextMenu();

        var addToColumn = new MenuItem { Header = "Add To Currently Selected Column" };
        addToColumn.Click += (sender, e) =>
        {
            if (CurrentColumn != null)
            {
                Button button = CreateLoopButton(loop, true);
                CurrentColumn.Items.Add(button);
                
                switch (CurrentColumn.Name)
                {
                    case "LoopCol1":
                        col1Loops.Add(loop);
                        buttonCol = col1Loops;
                        break;
                    case "LoopCol2":
                        col2Loops.Add(loop);
                        buttonCol = col2Loops;
                        break;
                    case "LoopCol3":
                        col3Loops.Add(loop);
                        buttonCol = col3Loops;
                        break;
                    case "LoopCol4":
                        col4Loops.Add(loop);
                        buttonCol = col4Loops;
                        break;
                }
            }
        };
        contextMenu.Items.Add(addToColumn);

        if (isSongColumn)
        {
            contextMenu.Items.Remove(addToColumn);
            var removeFromColumn = new MenuItem { Header = "Remove From Column" };
            removeFromColumn.Click += (sender, e) =>
            {
                CurrentColumn.Items.Remove(loopButton);
                buttonCol.Remove(loop);
            };
            contextMenu.Items.Add(removeFromColumn);
        }

        loopButton.ContextMenu = contextMenu;
        return loopButton;
    }

    private Button CreateCategoryButton(String content, List<Button> btnList)
    {
        var categoryButton = new Button
        {
            Content = content,
            Width = 70,
            Height = 30
        };

        categoryButton.Click += (sender, e) => 
        {
            DisplayButtons(btnList);
        };

        return categoryButton;
    }

    /// <summary>
    /// Displays new list of buttons based on category selected
    /// </summary>
    /// <param name="list">List of buttons to use</param>
    private void DisplayButtons(List<Button> list)
    {
        var buttonsContainer = this.FindControl<ItemsControl>("ButtonsControl");
        
        if (list != CurrentCategory)
        {
            buttonsContainer.Items.Clear();
            
            foreach (var button in list)
            {
                buttonsContainer.Items.Add(button);
            }

            CurrentCategory = list;

            Console.WriteLine("Loop Buttons Displayed");
        }
    }

    private void CreateMediaControl()
    {
        var playButton = new Button
        {
            Width = 50,
            Height = 50,
            CornerRadius = new CornerRadius(10)
        };
        
        var playButtonContent = new TextBlock
        {
            Text = "▶",
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        playButton.Content = playButtonContent;

        playButton.Click += async (sender, e) =>
        {
            await PlayColumnLoops();
        };

        var stopButton = new Button
        {
            Width = 50,
            Height = 50,
            CornerRadius = new CornerRadius(10)
        };

        var stopButtonContent = new TextBlock()
        {
            Text = "⏹",
            TextAlignment = TextAlignment.Center,
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
        };
        stopButton.Content = stopButtonContent;

        stopButton.Click += (sender, e) =>
        {
            audioPlayer.Stop();
        };
        
        var grid = this.FindControl<Grid>("Grid");
        
        var stackPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };

        stackPanel.Children.Add(playButton);
        stackPanel.Children.Add(stopButton);

        Grid.SetColumn(stackPanel, 1);
        Grid.SetRow(stackPanel, 1);
        grid.Children.Add(stackPanel);
    }

    /// <summary>
    /// Plays all the loops in order 2 times each.
    /// </summary>
    private async Task PlayColumnLoops()
    {
        List<List<LoopFile>> columns = new List<List<LoopFile>> { col1Loops, col2Loops, col3Loops, col4Loops };

        foreach (var column in columns)
        {
            List<Task> playTasks = new List<Task>();

            foreach (var loop in column)
            {
                playTasks.Add(Task.Run(() =>
                {
                    audioPlayer.Play(loop);
                    Task.Delay((int)loop.GetDuration()).Wait();
                    audioPlayer.Stop();
                }));
            }

            // Wait for all loops in the column to finish playing
            await Task.WhenAll(playTasks);
        }
    }
    
    private async void CreateCustomLoop(object? sender, RoutedEventArgs e)
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
            string destinationFilePath = Path.Combine(customDir, Path.GetFileName(sourceFilePath));

            File.Copy(sourceFilePath, destinationFilePath, true);

            CreateLoopButton(new LoopFile(destinationFilePath));
        }
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}