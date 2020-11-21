using MelonLoader;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GTFO.Custom.Rundown.CRundown
{
    [Obsolete]
    public static partial class DataBlocksDissolver
    {
        public static bool IsCustomDataLoaded { get; set; } = false;

        private static string _MD5Buffer = string.Empty;
        private static MD5 _MD5;

        static DataBlocksDissolver()
        {
            _MD5 = MD5.Create();
        }

        public static string FileToMD5(string path)
        {
            if (File.Exists(path))
            {
                var bytes = _MD5.ComputeHash(File.ReadAllBytes(path));
                var md5 = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                return md5;
            }

            return string.Empty;
        }

        public static RundownPair[] DissolveAllRundowns(string path)
        {
            try
            {
                List<RundownPair> rundowns = new List<RundownPair>();

                var dirs = Directory.GetDirectories(path);
                foreach (var dir in dirs)
                {
                    _MD5Buffer = string.Empty;

                    var manifestFile = Path.Combine(dir, "_manifest.json");
                    var rundownFile = Path.Combine(dir, "_rundown.json");

                    if (!File.Exists(manifestFile))
                    {
                        throw new FileNotFoundException($"Unable to found essential file '_manifest.json' for Custom Rundown: {Path.GetDirectoryName(dir)}");
                    }

                    if (!File.Exists(rundownFile))
                    {
                        throw new FileNotFoundException($"Unable to found essential file '_rundown.json' for Custom Rundown: {Path.GetDirectoryName(dir)}");
                    }

                    var manifast = JsonConvert.DeserializeObject<CRundownManifest>(File.ReadAllText(manifestFile));
                    if (string.IsNullOrEmpty(manifast.Name))
                    {
                        throw new FileNotFoundException($"Manifast file doesn't have any Name for Custom Rundown: {Path.GetDirectoryName(dir)}");
                    }

                    _MD5Buffer += FileToMD5(manifestFile);
                    _MD5Buffer += FileToMD5(rundownFile);

                    List<IDChangePair> fogChange = new List<IDChangePair>();
                    foreach (var fogFile in manifast.FogSettings)
                    {
                        var change = DissolveFogSetting(File.ReadAllText(Path.Combine(dir, fogFile)));
                        fogChange.Add(change);

                        _MD5Buffer += FileToMD5(fogFile);
                    }

                    List<IDChangePair> lightChange = new List<IDChangePair>();
                    foreach (var lightFile in manifast.LightSettings)
                    {
                        var change = DissolveLightSetting(File.ReadAllText(Path.Combine(dir, lightFile)));
                        lightChange.Add(change);

                        _MD5Buffer += FileToMD5(lightFile);
                    }

                    List<IDChangePair> puzzleChange = new List<IDChangePair>();
                    foreach (var puzzleFile in manifast.ChainedPuzzles)
                    {
                        var change = DissolveChainedPuzzle(File.ReadAllText(Path.Combine(dir, puzzleFile)));
                        puzzleChange.Add(change);

                        _MD5Buffer += FileToMD5(puzzleFile);
                    }

                    List<IDChangePair> objectiveChange = new List<IDChangePair>();
                    foreach (var objFile in manifast.WardenObjectives)
                    {
                        var change = DissolveWardenObjective(File.ReadAllText(Path.Combine(dir, objFile)));
                        objectiveChange.Add(change);

                        _MD5Buffer += FileToMD5(objFile);
                    }

                    var lightChangeArray = lightChange.ToArray();
                    var puzzleChangeArray = puzzleChange.ToArray();

                    List<IDChangePair> layoutChange = new List<IDChangePair>();
                    foreach (var layoutFile in manifast.LevelLayouts)
                    {
                        var change = DissolveLayout(File.ReadAllText(Path.Combine(dir, layoutFile)), lightChangeArray, puzzleChangeArray);
                        layoutChange.Add(change);

                        _MD5Buffer += FileToMD5(layoutFile);
                    }

                    var changePair = new RundownChangePair()
                    {
                        FogSettings = fogChange.ToArray(),
                        LightSettings = lightChangeArray,
                        Layouts = layoutChange.ToArray(),
                        Objectives = objectiveChange.ToArray()
                    };

                    var rdwChange = DissolveRundown(File.ReadAllText(rundownFile), changePair);
                    var bytes = _MD5.ComputeHash(Encoding.UTF8.GetBytes(_MD5Buffer));
                    var checksum = BitConverter.ToString(bytes).Replace("-", "").ToLower();

                    rundowns.Add(new RundownPair()
                    {
                        Name = manifast.Name,
                        ID = rdwChange.New,
                        Checksum = checksum
                    });
                }

                IsCustomDataLoaded = true;

                return rundowns.ToArray();
            }
            catch (Exception e)
            {
                MelonLogger.Log(e.ToString());
                return null;
            }
        }
    }
}