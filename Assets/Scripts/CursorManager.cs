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
    public Texture2D specialCursor;
    public Texture2D holdingCursor;
    public Texture2D leaveCursor;

    private Dictionary<(object source, bool holding), (CursorType type, int priority, string tooltip)> requests = new();

    private Vector3 hotspot = new Vector3(20, 20, 1);

    private object holder;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        requests[(this, false)] = (CursorType.Normal, (int)Prio.Base, "");
    }

    void Start() {
        Refresh();
    }

    public void SwitchCursors(HeroSpriteSet set) {
        normalCursor = set.normalCursor;
        grabCursor = set.grabCursor;
        attackCursor = set.attackCursor;
        specialCursor = set.specialCursor;
        holdingCursor = set.holdingCursor;
        leaveCursor = set.leaveCursor;
        Refresh();
    }

    public void AddRequest(object source, CursorType type, Prio priority, string tooltip = "", bool whileHolding = false) {
        requests[(source, whileHolding)] = (type, (int)priority, tooltip);
        Refresh();
    }

    public void StartHolding(object source) {
        holder = source;
        requests[(this, true)] = (CursorType.Holding, (int)Prio.Holding, "");
        Refresh();
    }

    public void StopHolding(object source) {
        if (holder != source) return;
        holder = null;
        requests.Remove((this, true));
        Refresh();
    }

    public void RemoveRequest(object source) {
        if (source == (object)this) return;
        requests.Remove((source, false));
        requests.Remove((source, true));
        Refresh();
    }

    void Refresh() {
        
        bool holding = holder != null;
        var top = requests.Where(kv => kv.Key.holding == holding).Select(kv => kv.Value).OrderByDescending(r => r.priority).First();

        Cursor.SetCursor(GetTexture(top.type), hotspot, CursorMode.Auto);
        CursorTextGO.Instance?.Show(top.tooltip);
    }

    Texture2D GetTexture(CursorType type) => type switch {
        CursorType.Grab   => grabCursor,
        CursorType.Attack => attackCursor,
        CursorType.Special => specialCursor,
        CursorType.Holding => holdingCursor,
        CursorType.Leave => leaveCursor,
        _                 => normalCursor
    };
}