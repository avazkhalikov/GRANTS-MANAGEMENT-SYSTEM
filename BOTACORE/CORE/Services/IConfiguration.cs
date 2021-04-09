using StructureMap;

namespace BOTACORE.CORE
{
   // [PluginFamily("Default")]
    public interface IConfiguration
    {
        string AdminSiteURL { get; }
    }
}
