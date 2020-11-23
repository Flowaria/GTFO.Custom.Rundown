namespace GTFO.Custom.Rundown.CRundown
{
    public class CRundown
    {
        public CRundownManifest Manifest { get; private set; }

        public static CRundown CreateFromFolder(string path)
        {
            var rundown = new CRundown();
            rundown.Manifest = new CRundownManifest();
            return null;
        }
    }
}