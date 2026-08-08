using UnityEngine;

// Carries the confirmed real-table world transform from the AR scene (TableAR)
// to the VR scene (ForeverSnowland Stylus). Plain static class, not a
// MonoBehaviour/DontDestroyOnLoad singleton: static fields already survive a
// SceneManager.LoadScene call within the same play session, so no GameObject
// or scene wiring is needed to keep this data alive across the handoff.
public static class TableAnchor
{
    // True once Save() has been called this session. VR scene checks this
    // before moving Base Game, so it never gets repositioned on a normal
    // launch straight into the VR scene (no AR scan happened).
    public static bool HasValue { get; private set; }

    public static Vector3 Position { get; private set; }
    public static Quaternion Rotation { get; private set; }

    // Called from the AR scene right when the player confirms table placement.
    public static void Save(Transform confirmedTable)
    {
        Position = confirmedTable.position;
        Rotation = confirmedTable.rotation;
        HasValue = true;
    }
}
