using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChasBWare.SpotLight.Definitions.ViewModels;
using ChasBWare.SpotLight.Domain.Entities;
using ChasBWare.SpotLight.Infrastructure.Utility;

namespace ChasBWare.SpotLight.Definitions.Tasks.Recorder;

public class ParseClipboardTask(IDispatcher _dispatcher) 
             : IParseClipboardTask
{
    /// <summary>
    /// expects text to bee in tab seperated list 
    /// see ExportPlaylistTask write track for details
    /// </summary>
    /// <param name="viewModel"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public void Execute(IRecorderViewModel recorder)
    {
        var text = Clipboard.Default.GetTextAsync().Result;
        if (string.IsNullOrWhiteSpace(text))
        {
            return;
        }

        Task.Run(() => RunParseClipboard(recorder, text));
    }

    private void RunParseClipboard(IRecorderViewModel recorder, string text)
    {
        
        List<Track> list = [];
        list.ParseTracks(text);

        _dispatcher.Dispatch(() =>
        {
            recorder.AddTracks(list);
        });
    }
}
