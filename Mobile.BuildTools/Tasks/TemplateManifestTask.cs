using System;
using System.IO;
using Mobile.BuildTools.Generators;
using Mobile.BuildTools.Logging;

namespace Mobile.BuildTools.Tasks
{
    public class TemplateManifestTask : Microsoft.Build.Utilities.Task
    {
        public string Prefix { get; set; }

        public string Token { get; set; }

        public string ProjectDirectory { get; set; }

        public string ManifestTemplatePath { get; set; }

        public string ManifestOutputPath { get; set; }

        public string SdkShortFrameworkIdentifier { get; set; }

        public string DebugOutput { get; set; }

        public override bool Execute()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ManifestOutputPath) || string.IsNullOrWhiteSpace(ManifestTemplatePath))
                {
                    Log.LogMessage($"No Template path specified for {SdkShortFrameworkIdentifier}");
                }
                else if (File.Exists(ManifestTemplatePath))
                {
                    Log.LogMessage($"Generating '{Path.GetFileName(ManifestOutputPath)}'");
                    bool.TryParse(DebugOutput, out var debug);
                    var generator = new AppManifestGenerator()
                    {
                        Prefix = Prefix,
                        Token = Token,
                        SdkShortFrameworkIdentifier = SdkShortFrameworkIdentifier,
                        ProjectDirectory = ProjectDirectory,
                        ManifestOutputPath = ManifestOutputPath,
                        ManifestTemplatePath = ManifestTemplatePath,
                        DebugOutput = debug,
                        Log = (BuildHostLoggingHelper)Log,
                    };

                    generator.Execute();
                }
                else
                {
                    Log.LogWarning($"Could not locate template manifest at '{ManifestTemplatePath}'");
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);

                return false;
            }
            return true;
        }
    }
}