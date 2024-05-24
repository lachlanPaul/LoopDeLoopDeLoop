using System;
using System.IO;
using LibVLCSharp.Shared;

namespace LoopDeLoopDeLoop.Components;

public class AudioPlayer    
{
    private static LibVLC libVLC = new LibVLC();
    private MediaPlayer _mediaPlayer = new MediaPlayer(libVLC);

    public void Play(LoopFile loopToPlay)
    {
        try
        {
            _mediaPlayer.Media = new Media(libVLC, loopToPlay.GetFilePath(), FromType.FromPath);
            _mediaPlayer.Play();
        }
        catch (FileNotFoundException)
        {
            System.Diagnostics.Debug.WriteLine($"{loopToPlay.GetFilePath()} could not be found!");
            
            // Running this to clean up, just in case
            Stop();
        }
    }

    /// <summary>
    /// Stops whatever is playing, and also dispose of the currently set audio file to save memory and stop leaks.
    /// </summary>
    public void Stop()
    {
        _mediaPlayer.Stop();
        
        // Release the media to save memory and prevent leaks
        _mediaPlayer.Media?.Dispose();
        _mediaPlayer.Media = null;
    }
}