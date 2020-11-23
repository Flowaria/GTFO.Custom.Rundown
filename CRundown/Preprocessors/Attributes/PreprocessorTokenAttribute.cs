using System;

namespace GTFO.Custom.Rundown.CRundown.Preprocessors.Attributes
{
    public class PreprocessorTokenAttribute : Attribute
    {
        public string Token;

        public PreprocessorTokenAttribute(string token)
        {
            Token = token;
        }
    }
}