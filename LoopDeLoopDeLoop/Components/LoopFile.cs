using System;
using System.Linq;
using LibVLCSharp.Shared;

namespace LoopDeLoopDeLoop.Components;

public class LoopFile
{
    private string FilePath;
    private string FileName;
    private string Category;
    private long Duration;
    private int BPM;
    
    public LoopFile(string fp)
    {
        FilePath = fp;
        FileName = FindLoopName();
        Category = FindLoopCategory();
        Duration = FindLoopDuration();
        BPM = FindLoopBPM();
        
        // Console.WriteLine($"Successfully processed {FileName}");
    }

    /// <summary>
    /// Find the loop's individual file name
    /// </summary>
    /// <returns>
    /// The loop's file name
    /// </returns>
    private string FindLoopName()
    {
        for (int i = FilePath.Length - 1; i >= 0; i--)
        {
            // The @ symbol stops C# from seeing the \ as an escape character.
            // We need to do this because C# outputs the file paths with backwards slashes.
            if (FilePath[i].ToString() == @"\")
            {
                // The first argument removes the file path before the name.
                // The second argument removes the .wav file extension.
                return FilePath.Substring(i + 1, FilePath.Length - i - 5);
            }
        }
        #warning Something went wrong and the loop file name couldn't be found, defaulting to the full path.
        return FilePath;
    }

    /// <summary>
    /// Since all loops are stored in instrument specific folders, so we can find it by parsing the path until we find a match.
    /// This also includes any custom tracks, kept under Loops/Custom.
    /// </summary>
    /// <returns>
    /// The Loop Instrument/Category
    /// </returns>
    private string FindLoopCategory()
    {
        string[] category = ["Drums", "Bass", "Guitar", "Piano", "Synth", "Custom"];
        string[] segments = FilePath.Split(@"\");
        
        for (int i = 0; i < category.Length; i++)
        {
            if (segments[2] == category[i])
            {
                return segments[2];
            }
        }

        #warning Suitable category could not be found, setting to "Undefined"
        return "Undefined";
    }

    private long FindLoopDuration()
    {
        var vlc = new LibVLC();
        var media = new Media(vlc, FilePath, FromType.FromPath);
        return media.Duration;
    }
    
    /// <summary>
    /// Most of the loops provided have their BPM listed in their file name within the first few characters.
    /// Since we can assume they have this trait, we scan the file name for the first instance of a number,
    /// and scan each subsequent number until we have a complete BPM track.
    /// This method also removes the BPM from the FileName variable.
    /// </summary>
    /// <returns>
    /// An int representing the BPM.
    /// </returns>
    private int FindLoopBPM()
    {
        string bpm = "";
        for (int i = 0; i < FileName.Length; i++)
        {
            if (Char.IsDigit(FileName[i]))
            {
                bpm += FileName[i];
                for (int j = i + 1; j < FileName.Length; j++)
                {
                    if (Char.IsDigit(FileName[j]))
                    {
                        bpm += FileName[j];
                    }
                    else
                    {
                        // We can use the non-int we just found as a reference point to where we can chop off the BPM.
                        FileName = FileName.Substring(FileName.IndexOf(FileName[j]));
                        FileName = FileName.Trim();
                        
                        break;
                    }
                }
                return Int32.Parse(bpm);
            }
        }
        // If we reach this point, no BPM was found, so we return 0.
        #warning Something went wrong and a loop's BPM could not be found, setting to 0
        return 0;
    }

    public string GetFileName()
    {
        return FileName;
    }

    public string GetFilePath()
    {
        return FilePath;
    }

    public string GetCategory()
    {
        return Category;
    }

    public long GetDuration()
    {
        return Duration;
    }

    public int GetBPM()
    {
        return BPM;
    }
    
    /// <summary>
    /// Prints all information on the Loop File to the console. Used for debugging
    /// </summary>
    public void OutputLoopInfo()
    {
        Console.WriteLine($"\nName: {FileName}");
        Console.WriteLine($"File Path: {FilePath}");
        Console.WriteLine($"Category: {Category}");
        Console.WriteLine($"Duration: {Duration}");
        Console.WriteLine($"BPM: {BPM}\n");
    }
}