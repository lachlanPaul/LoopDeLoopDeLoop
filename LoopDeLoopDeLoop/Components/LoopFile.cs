using System;
using System.Linq;

namespace LoopDeLoopDeLoop.Components;

public class LoopFile
{
    public string FilePath;
    public string FileName;
    public int BPM;
    
    public LoopFile(string fp)
    {
        FilePath = fp;
        FileName = FindLoopName();
        BPM = FindLoopBPM();
        
        Console.WriteLine($"Successfully processed {FileName}");
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
            if (FilePath[i].ToString() == "/")
            {
                return FilePath.Substring(i + 1);
            }
        }
        #warning Something went wrong and the loop file name couldn't be found, defaulting to the full path.
        return FilePath;
    }
    
    /// <summary>
    /// Most of the loops provided have their BPM listed in their file name within the first few characters.
    /// Since we can assume they have this trait, we scan the file name for the first instance of a number,
    /// and scan each subsequent number until we have a complete BPM track.
    /// </summary>
    /// <returns>
    /// An int representing the BPM.
    /// </returns>
    private int FindLoopBPM()
    {
        string bpm = "";
        
        for (int i = FileName.Length; i >= 0; i++)
        {
            if (Char.IsDigit(FileName[i]))
            {
                bpm = bpm + FileName[i];
                for (int j = FileName.Length - i; j >= 0; j--)
                {
                    if (Char.IsDigit(FileName[j]))
                    {
                        bpm = bpm + FileName[j];
                    }
                    else
                    {
                        return Int32.Parse(bpm);
                    }
                }
            }
        }
        
        // When a loop's BPM is 0, it will be sorted into a ? category.
        #warning Something went wrong and a loop's BPM could not be found, adding to the ? catergory
        return 0;
    }
}