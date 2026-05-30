/**
 * Cursor switching
 */

using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CursorManager : MonoBehaviour {
    public static CursorManager Instance { get; private set; }

    public Texture2D normalCursor;
    public Texture2D grabCursor;
    public Texture2D attackCursor;

    private Dictionary<object, (CursorType type, int priority, string TooltipText)> requests = new();

    private Vector3 hotspot = new Vector3(20, 20, 1);

    void Awake() {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start() {
        requests[this] = (CursorType.Normal, (int)Prio.Base, "");
        Refresh();
    }

    public void AddRequest(object source, CursorType type, Prio priority, string tooltip = "") {
        requests[source] = (type, (int)priority, tooltip);
        Refresh();
    }

    public void RemoveRequest(object source) {
        requests.Remove(source);
        Refresh();
    }

    void Refresh() {
        var top = requests.Values.OrderByDescending(r => r.priority).First();
        Cursor.SetCursor(GetTexture(top.type), hotspot, CursorMode.Auto);

        CursorTextGO.Instance?.Show(top.TooltipText);
    }

    Texture2D GetTexture(CursorType type) => type switch {
        CursorType.Grab   => grabCursor,
        CursorType.Attack => attackCursor,
        _                 => normalCursor
    };
}