using System;
using LibVLCSharp.Shared;

namespace LoopDeLoopDeLoop.Components;

public class AudioPlayer    
{
    private static LibVLC libVLC = new LibVLC();
    private MediaPlayer _mediaPlayer = new MediaPlayer(libVLC);

    public void Play(LoopFile loopToPlay)
    {
        _mediaPlayer.Media = new Media(libVLC, loopToPlay.GetFilePath(), FromType.FromPath);
        _mediaPlayer.Play();
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