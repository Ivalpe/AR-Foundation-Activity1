using UnityEngine.XR.Management;

public static class LoaderUtility
{
    /// <summary>
    /// Get the 'active' loader from XR Management.
    /// </summary>
    /// <returns>Returns the currently active `XRLoader`.</returns>
    public static XRLoader GetActiveLoader()
    {
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
        {
            return XRGeneralSettings.Instance.Manager.activeLoader;
        }

        return null;
    }

    /// <summary>
    /// Initializes the currently active `XR Loader`, if one exists. This creates all subsystems.
    /// </summary>
    /// <returns>`true` if there is an active loader and its `Initialize` and `Start` methods return `true`.
    /// Otherwise,`false`.</returns>
    public static bool Initialize()
    {
        var loader = GetActiveLoader();
        return loader && loader.Initialize() && loader.Start();
    }

    /// <summary>
    /// Deinitializes the currently active `XR Loader`, if one exists. This destroys all subsystems.
    /// </summary>
    /// <returns>`true` if there is an active loader and its `Stop` and `Deinitialize` methods return `true`.
    /// Otherwise, `false`.</returns>
    public static bool Deinitialize()
    {
        var loader = GetActiveLoader();
        return loader && loader.Stop() && loader.Deinitialize();
    }
}