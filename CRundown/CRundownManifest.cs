namespace GTFO.Custom.Rundown.CRundown
{
    public class CRundownManifest
    {
        public string Name { get; set; }
        public string GUID { get; set; }

        public string[] IncludedMacros; //File names for MacroDefinition JSON

        public string[] LevelLayouts; //File names for Level Layout JSON
        public string[] WardenObjectives; //And so on
        public string[] LightSettings;
        public string[] FogSettings;
        public string[] ChainedPuzzles;
        public string[] WavePopulations;
        public string[] WaveSettings;
    }
}