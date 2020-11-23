namespace GTFO.Custom.Rundown.CRundown.Preprocessors
{
    public interface IPreprocessor
    {
        bool TryGetContent(string[] args, out string content);
    }
}