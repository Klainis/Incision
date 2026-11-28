using Incision.Interfaces;
using Incision.Managers;
using UnityEngine;
using VContainer;

namespace Incision.Arcana
{
    public sealed class ArcanaRewardView : MonoBehaviour
    {
        private ArcanaRewardController _reward;
        private ArcanaLoadout _loadout;
        private ArcanaCombatModifiers _combatModifiers;
        private PauseManager _pause;
        private IEventBus _events;
        private bool _modalVisible;
        private bool _pausedByView;
        private bool _previousCursorVisibility;
        private CursorLockMode _previousCursorLock;
        private Vector2 _scroll;
        private GUIStyle _hudTitle;
        private GUIStyle _hudDetail;
        private GUIStyle _hudTimer;
        private GUIStyle _modalHeading;
        private GUIStyle _cardName;
        private GUIStyle _cardDetail;
        private GUIStyle _cardAction;

        [Inject]
        private void Initialize(
            ArcanaRewardController reward, ArcanaLoadout loadout, ArcanaCombatModifiers combatModifiers,
            PauseManager pause, IEventBus events)
        {
            _reward = reward;
            _loadout = loadout;
            _combatModifiers = combatModifiers;
            _pause = pause;
            _events = events;
            _events.Subscribe<ArcanaRewardChanged>(OnRewardChanged);
            SetModalVisibility(_reward.IsOpen);
        }

        private void OnDestroy()
        {
            _events?.Unsubscribe<ArcanaRewardChanged>(OnRewardChanged);
            SetModalVisibility(false);
        }

        private void OnRewardChanged(ArcanaRewardChanged evt)
        {
            _scroll = Vector2.zero;
            SetModalVisibility(_reward.IsOpen);
        }

        private void SetModalVisibility(bool visible)
        {
            if (_modalVisible == visible)
                return;

            _modalVisible = visible;
            if (visible)
            {
                _previousCursorVisibility = Cursor.visible;
                _previousCursorLock = Cursor.lockState;
                _pausedByView = !_pause.IsPaused;
                _pause.Pause();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }

            Cursor.lockState = _previousCursorLock;
            Cursor.visible = _previousCursorVisibility;
            if (_pausedByView)
                _pause.Resume();
            _pausedByView = false;
        }

        private void OnGUI()
        {
            if (_reward == null)
                return;

            _modalHeading ??= CreateModalLabel(22, FontStyle.Normal);
            _cardName ??= CreateModalLabel(17, FontStyle.Bold);
            _cardDetail ??= CreateModalLabel(13, FontStyle.Normal);
            _cardAction ??= CreateModalLabel(13, FontStyle.Bold);

            if (_modalVisible)
                DrawReward();
            else
                DrawSlots();
        }

        private void DrawSlots()
        {
            const float margin = 16f;
            const float gap = 6f;
            const float width = 112f;
            const float height = 52f;
            int columns = Mathf.Max(1, Mathf.FloorToInt((Screen.width * 0.6f - margin) / (width + gap)));
            float now = Time.time;
            int index = 0;
            foreach (var slot in _loadout.Slots)
            {
                if (slot.Definition == null)
                    continue;

                var tile = new Rect(Screen.width - margin - width - index % columns * (width + gap),
                    margin + index / columns * (height + gap), width, height);
                DrawHudTile(tile, slot, now);
                index++;
            }
        }

        private void DrawHudTile(Rect tile, ArcanaSlot slot, float now)
        {
            _hudTitle ??= CreateHudLabel(12, FontStyle.Bold, TextAnchor.MiddleLeft, Color.white);
            _hudDetail ??= CreateHudLabel(11, FontStyle.Normal, TextAnchor.MiddleLeft, new Color(0.76f, 0.78f, 0.84f));
            _hudTimer ??= CreateHudLabel(12, FontStyle.Bold, TextAnchor.MiddleRight, new Color(1f, 0.83f, 0.48f));

            var definition = slot.Definition;
            var effect = definition.Effect;
            float remaining = effect != null && effect.Duration > 0f
                ? _combatModifiers.GetRemainingDuration(effect.Kind, now)
                : 0f;
            bool active = remaining > 0f;
            var accent = active ? new Color(0.98f, 0.71f, 0.3f) : new Color(0.48f, 0.4f, 0.66f);
            DrawSurface(tile, active ? new Color(0.24f, 0.17f, 0.09f, 0.96f) : new Color(0.11f, 0.09f, 0.16f, 0.93f));
            DrawSurface(new Rect(tile.x, tile.y, 3f, tile.height), accent);

            var title = new Rect(tile.x + 8f, tile.y + 3f, tile.width - 15f, 24f);
            var details = new Rect(tile.x + 8f, tile.y + 28f, tile.width - 15f, 16f);
            GUI.Label(title, definition.DisplayName, _hudTitle);
            GUI.Label(details, $"Lv. {slot.Level}", _hudDetail);

            if (!active)
                return;

            GUI.Label(details, $"{Mathf.Ceil(remaining * 10f) / 10f:0.0}s", _hudTimer);
            float fraction = Mathf.Clamp01(remaining / effect.Duration);
            DrawSurface(new Rect(tile.x + 3f, tile.yMax - 3f, (tile.width - 3f) * fraction, 3f), accent);
        }

        private static GUIStyle CreateHudLabel(int size, FontStyle fontStyle, TextAnchor alignment, Color color)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = alignment,
                fontSize = size,
                fontStyle = fontStyle,
                wordWrap = true
            };
            style.normal.textColor = color;
            return style;
        }

        private void DrawReward()
        {
            const float panelWidth = 688f;
            const float panelHeight = 386f;
            var scale = Mathf.Min(Screen.width * 0.88f / panelWidth, Screen.height * 0.85f / panelHeight);
            if (scale <= 0f)
                return;

            DrawSurface(new Rect(0f, 0f, Screen.width, Screen.height), new Color(0.06f, 0.06f, 0.06f, 0.9f));

            var previousMatrix = GUI.matrix;
            GUI.matrix = previousMatrix * Matrix4x4.Scale(new Vector3(scale, scale, 1f));
            var panel = new Rect(
                (Screen.width / scale - panelWidth) * 0.5f,
                (Screen.height / scale - panelHeight) * 0.5f,
                panelWidth,
                panelHeight);
            DrawSurface(panel, new Color(0.98f, 0.98f, 0.98f));

            GUI.BeginGroup(panel);
            var selection = _reward.Selected;
            GUI.Label(new Rect(24f, 38f, panelWidth - 48f, 38f),
                selection == null ? "Choose an Arcana" : $"Replace an Arcana with {selection.DisplayName}", _modalHeading);
            int choice = selection == null ? DrawOffers(panelWidth) : DrawReplacementSlots(panelWidth, selection);
            bool decline = GUI.Button(new Rect((panelWidth - 168f) * 0.5f, 326f, 168f, 30f), "Decline reward");
            GUI.EndGroup();
            GUI.matrix = previousMatrix;

            if (decline)
                _reward.Decline();
            else if (choice >= 0)
            {
                if (selection == null)
                    _reward.Select(choice);
                else
                    _reward.Replace(choice);
            }
        }

        private int DrawOffers(float panelWidth)
        {
            const float cardWidth = 132f;
            const float cardHeight = 204f;
            const float gap = 32f;
            int count = _reward.Offers.Count;
            if (count == 0)
                return -1;

            float rowWidth = cardWidth * count + gap * (count - 1);
            float startX = (panelWidth - rowWidth) * 0.5f;
            int choice = -1;
            for (int i = 0; i < count; i++)
            {
                var offer = _reward.Offers[i];
                int level = GetOwnedLevel(offer);
                var card = new Rect(startX + i * (cardWidth + gap), 102f, cardWidth, cardHeight);
                bool hovered = card.Contains(Event.current.mousePosition);
                DrawSurface(card, hovered ? new Color(0.75f, 0.75f, 0.75f) : new Color(0.85f, 0.85f, 0.85f));
                if (GUI.Button(card, GUIContent.none, GUIStyle.none))
                    choice = i;

                float x = card.x + 8f;
                float textWidth = card.width - 16f;
                GUI.Label(new Rect(x, card.y + 9f, textWidth, 19f), offer.Rarity.ToString(), _cardDetail);
                GUI.Label(new Rect(x, card.y + 31f, textWidth, 36f), offer.DisplayName, _cardName);
                GUI.Label(new Rect(x, card.y + 66f, textWidth, 63f), offer.Description, _cardDetail);
                string bonus = level == 0 ? offer.GetBonusDescription(1) : offer.GetBonusDescription(level, level + 1);
                GUI.Label(new Rect(x, card.y + 129f, textWidth, 47f), bonus, _cardAction);
                string action = level == 0 ? "Take Arcana" : $"Upgrade to Lv. {level + 1}";
                GUI.Label(new Rect(x, card.y + 178f, textWidth, 20f), action, _cardDetail);
            }

            return choice;
        }

        private int DrawReplacementSlots(float panelWidth, ArcanaDefinition incoming)
        {
            GUI.Label(new Rect(24f, 77f, panelWidth - 48f, 26f),
                $"Gain: {incoming.GetBonusDescription(1)}", _cardAction);
            int choice = -1;
            GUILayout.BeginArea(new Rect((panelWidth - 420f) * 0.5f, 110f, 420f, 184f));
            _scroll = GUILayout.BeginScrollView(_scroll);
            for (int i = 0; i < _loadout.Slots.Count; i++)
            {
                var slot = _loadout.Slots[i];
                if (slot.Definition == null)
                    continue;

                string label = $"Replace {slot.Definition.DisplayName} · Lv. {slot.Level}\n" +
                    $"Lose: {slot.Definition.GetBonusDescription(slot.Level)}";
                if (GUILayout.Button(label, GUILayout.MinHeight(42f)))
                    choice = i;
            }
            GUILayout.EndScrollView();
            GUILayout.EndArea();
            return choice;
        }

        private static GUIStyle CreateModalLabel(int size, FontStyle fontStyle)
        {
            var style = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = size,
                fontStyle = fontStyle,
                wordWrap = true
            };
            style.normal.textColor = new Color(0.12f, 0.12f, 0.12f);
            return style;
        }

        private static void DrawSurface(Rect bounds, Color color)
        {
            var previousColor = GUI.color;
            GUI.color = color;
            GUI.DrawTexture(bounds, Texture2D.whiteTexture);
            GUI.color = previousColor;
        }

        private int GetOwnedLevel(ArcanaDefinition offer)
        {
            foreach (var slot in _loadout.Slots)
            {
                if (slot.Definition?.Id == offer.Id)
                    return slot.Level;
            }

            return 0;
        }
    }
}
