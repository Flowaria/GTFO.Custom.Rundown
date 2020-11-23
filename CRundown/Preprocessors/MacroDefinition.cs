using GTFO.Custom.Rundown.CRundown.Preprocessors.Attributes;

namespace GTFO.Custom.Rundown.CRundown.Preprocessors
{
    [PreprocessorToken("macro")]
    public class MacroDefinition : IPreprocessor
    {
        public string Token;
        public string Content;
        public string[] Arguments;

        public int ArgsLength
        {
            get { return Arguments?.Length ?? 0; }
        }

        public bool TryGetContent(string[] args, out string content)
        {
            if (args.Length != ArgsLength)
            {
                content = string.Empty;
                return false;
            }

            content = "";
            return true;
        }
    }

    public class MacroDefinitionWrapper
    {
        public MacroDefinition[] MacroDefinitions;
    }
}