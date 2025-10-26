using System.Diagnostics;
using System.IO;
using UnityEngine;

public class AppReloader : MonoBehaviour
{
    public static void Restart()
    {
        // Zur Sicherheit: Physik & Time normalisieren
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        // --- Neuen Prozess starten (plattformabhängig) ---
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        string exePath = Process.GetCurrentProcess().MainModule!.FileName!;
        var psi = new ProcessStartInfo(exePath)
        {
            UseShellExecute = false,
            WorkingDirectory = Path.GetDirectoryName(exePath)
        };
        Process.Start(psi);
#elif UNITY_STANDALONE_OSX
        // dataPath ist .../MyGame.app/Contents; wir wollen den .app Pfad
        string appPath = Application.dataPath;
        int idx = appPath.LastIndexOf(".app/");
        if (idx >= 0) appPath = appPath.Substring(0, idx + 4);
        var psi = new ProcessStartInfo("/usr/bin/open", $"-n \"{appPath}\"")
        {
            UseShellExecute = false
        };
        Process.Start(psi);
#elif UNITY_STANDALONE_LINUX
        string exePath = Process.GetCurrentProcess().MainModule!.FileName!;
        var psi = new ProcessStartInfo(exePath)
        {
            UseShellExecute  = false,
            WorkingDirectory = Path.GetDirectoryName(exePath)
        };
        Process.Start(psi);
#endif

        // Mini-Delay kann helfen, damit der neue Prozess "voraus" ist
        // (optional – je nach OS/AV nicht nötig)
        // UnityMainThreadDispatcher.StartCoroutine(QuitNextFrame());

        // --- Aktuellen Prozess beenden ---
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
