namespace Blueprints.Boot
{
    /// <summary>
    /// Boot up for all manager classes this needs to be put on an object in the scene and will create the singleton instance of anything added 
    /// </summary>
    public class BootUp : TBoot
    {
        private void Awake() => Boot();
    }
}
