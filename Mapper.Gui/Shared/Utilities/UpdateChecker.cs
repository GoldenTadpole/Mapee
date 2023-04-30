using AutoUpdaterDotNET;

namespace Mapper.Gui
{
    public static class UpdateChecker
    {
        private static readonly string _versionInfoUrl = "https://storage.googleapis.com/mapee_update_bucket_goldentadpole/self_win64/MapeeVersionInfo-self-win64.xml";

        public static void Check() 
        {
            AutoUpdater.Start(_versionInfoUrl);
        }
    }
}
