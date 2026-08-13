using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;

// AR Foundation's ARInputManager owns the XRInputSubsystem lifetime: it starts the
// subsystem in OnEnable and stops it in OnDisable. Unloading the AR scene therefore
// leaves the subsystem stopped, which freezes every HMD and controller pose until the
// OS restarts it on app resume (the reason a trip to the system menu "fixes" it).
//
// This guard installs itself once per session and keeps the subsystem running:
// on every scene load - which happens after all Awake/OnEnable and before any Start,
// so XROrigin still sees a live subsystem when it applies its tracking origin mode -
// and every frame as a fallback for any other cause.
public class XRInputSubsystemGuard : MonoBehaviour
{
    private const int FailedAttemptsBeforeError = 120;

    private static XRInputSubsystemGuard instance;

    private int failedAttempts;

    // For callers that stop the subsystem knowingly (see ConfirmTablePlacement) and
    // need it back within the same frame instead of on the next scene load.
    public static void RestoreNow()
    {
        if (instance != null)
            instance.EnsureRunning();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        if (instance != null)
            return;

        GameObject host = new GameObject(nameof(XRInputSubsystemGuard));
        DontDestroyOnLoad(host);

        instance = host.AddComponent<XRInputSubsystemGuard>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        EnsureRunning();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        EnsureRunning();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureRunning();
    }

    private void EnsureRunning()
    {
        XRInputSubsystem inputSubsystem = GetActiveInputSubsystem();

        if (inputSubsystem == null || inputSubsystem.running)
        {
            failedAttempts = 0;
            return;
        }

        inputSubsystem.Start();

        if (inputSubsystem.running)
        {
            failedAttempts = 0;
            Debug.LogWarning($"{nameof(XRInputSubsystemGuard)}: restarted a stopped XRInputSubsystem - tracking restored.");
            return;
        }

        failedAttempts++;

        if (failedAttempts == FailedAttemptsBeforeError)
            Debug.LogError($"{nameof(XRInputSubsystemGuard)}: XRInputSubsystem will not start after {FailedAttemptsBeforeError} attempts - head and controller tracking stays frozen.");
    }

    private static XRInputSubsystem GetActiveInputSubsystem()
    {
        if (XRGeneralSettings.Instance == null || XRGeneralSettings.Instance.Manager == null)
            return null;

        XRLoader loader = XRGeneralSettings.Instance.Manager.activeLoader;

        return loader == null ? null : loader.GetLoadedSubsystem<XRInputSubsystem>();
    }
}
