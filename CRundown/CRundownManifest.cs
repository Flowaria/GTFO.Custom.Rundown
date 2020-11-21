using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTFO.Custom.Rundown.CRundown
{
    public class CRundownManifest
    {
        public string Name { get; set; }
        public string GUID { get; set; }

        public string[] LevelLayouts;
        public string[] WardenObjectives;
        public string[] LightSettings;
        public string[] FogSettings;
        public string[] ChainedPuzzles;
        public string[] WavePopulations;
        public string[] WaveSettings;
    }
}
