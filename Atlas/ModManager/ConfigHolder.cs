using Atlas.ModManager.Configs.Atlas;
using Atlas.ModManager;

/// <summary>
/// Used to manage Atlas configuration.
/// </summary>
public static class ConfigHolder
{
    /// <summary>
    /// Gets the <see cref="ServerConfiguration"/> instance.
    /// </summary>
    public static readonly ServerConfiguration Server = new ServerConfiguration();
    
    /// <summary>
    /// Gets the <see cref="AtlasConfiguration"/> instance.
    /// </summary>
    public static readonly AtlasConfiguration Atlas = new AtlasConfiguration();

    /// <summary>
    /// Gets the <see cref="RoleConfiguration"/> instance.
    /// </summary>
    public static readonly RoleConfiguration Role = new RoleConfiguration();

    /// <summary>
    /// Gets the <see cref="MapConfiguration"/> instance.
    /// </summary>
    public static readonly MapConfiguration Map = new MapConfiguration();

    /// <summary>
    /// Gets the <see cref="Scp939Configuration"/> instance.
    /// </summary>
    public static readonly Scp939Configuration Scp939 = new Scp939Configuration();

    /// <summary>
    /// Gets the <see cref="Scp173Configuration"/> instance.
    /// </summary>
    public static readonly Scp173Configuration Scp173 = new Scp173Configuration();

    /// <summary>
    /// Gets the <see cref="Scp106Configuration"/> instance.
    /// </summary>
    public static readonly Scp106Configuration Scp106 = new Scp106Configuration();

    /// <summary>
    /// Gets the <see cref="Scp914Configuration"/> instance.
    /// </summary>
    public static readonly Scp914Configuration Scp914 = new Scp914Configuration();

    /// <summary>
    /// Gets the <see cref="Scp049Configuration"/> instance.
    /// </summary>
    public static readonly Scp049Configuration Scp049 = new Scp049Configuration();

    /// <summary>
    /// Gets the <see cref="Scp079Configuration"/> instance.
    /// </summary>
    public static readonly Scp079Configuration Scp079 = new Scp079Configuration();

    /// <summary>
    /// Gets the <see cref="Scp096Configuration"/> instance.
    /// </summary>
    public static readonly Scp096Configuration Scp096 = new Scp096Configuration();

    /// <summary>
    /// Reloads all Atlas configs.
    /// </summary>
    public static void Reload()
    {
        ConfigManager.Reload(Server);
        ConfigManager.Reload(Atlas);
        ConfigManager.Reload(Role);
        ConfigManager.Reload(Map);

        ConfigManager.Reload(Scp939);
        ConfigManager.Reload(Scp173);
        ConfigManager.Reload(Scp106);
        ConfigManager.Reload(Scp914);
        ConfigManager.Reload(Scp049);
        ConfigManager.Reload(Scp079);
        ConfigManager.Reload(Scp096);
    }
}
