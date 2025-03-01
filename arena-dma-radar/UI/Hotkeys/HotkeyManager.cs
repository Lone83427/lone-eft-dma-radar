using arena_dma_radar.UI.Radar;
using arena_dma_radar.UI.Misc;
using DarkModeForms;
using eft_dma_shared.Common.Misc;
using eft_dma_shared.Common.Unity;

namespace arena_dma_radar.UI.Hotkeys
{
    public sealed partial class HotkeyManager : Form
    {
        private readonly DarkModeCS _darkmode;
        private HotkeyActionController _selectedAction;
        private UnityKeyCode? _selectedHotkey;
        private HotkeyListBoxEntry _selectedEntry;

        public HotkeyManager()
        {
            if (_parent is null)
                throw new InvalidOperationException("Hotkey Manager has not been initialized!");
            InitializeComponent();
            _darkmode = new DarkModeCS(this);
            foreach (var key in _allKeys)
                comboBox_Hotkeys.Items.Add(new ComboHotkeyValue(key));
            foreach (var actionController in _actionControllers)
                comboBox_Actions.Items.Add(actionController);
            foreach (var entry in _hotkeys)
            {
                var hotkey = entry.Key;
                var action = entry.Value;
                listBox_Values.Items.Add(new HotkeyListBoxEntry(hotkey, action));
            }
        }

        private void button_Add_Click(object sender, EventArgs e)
        {
            if (_selectedHotkey is not UnityKeyCode keyCode ||
                _selectedAction is not HotkeyActionController actionController)
                return;
            var existingItems = listBox_Values.Items.Cast<HotkeyListBoxEntry>().ToList();
            if (existingItems.Any(x => x.Action.Name == actionController.Name)
                || existingItems.Any(x => x.Hotkey == keyCode))
            {
                MessageBox.Show(this, "Hotkey/Action already in use!");
                return;
            }
            var item = new HotkeyListBoxEntry(keyCode, new(actionController.Name));
            listBox_Values.Items.Add(item);
            comboBox_Actions.SelectedIndex = -1;
            comboBox_Hotkeys.SelectedIndex = -1;
        }

        private void button_Remove_Click(object sender, EventArgs e)
        {
            if (_selectedEntry is not HotkeyListBoxEntry entry)
                return;
            listBox_Values.Items.Remove(entry);
            listBox_Values.SelectedIndex= -1;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            var hotkeys = new ConcurrentDictionary<UnityKeyCode, HotkeyAction>();
            var hotkeysToDict = new Dictionary<int, string>();
            foreach (var item in listBox_Values.Items)
                if (item is HotkeyListBoxEntry entry)
                {
                    hotkeys.TryAdd(entry.Hotkey, entry.Action);
                    hotkeysToDict.TryAdd((int)entry.Hotkey, entry.Action.Name);
                }
            // ref swaps
            _hotkeys = hotkeys;
            Config.Hotkeys = hotkeysToDict;
            // return to caller
            this.DialogResult = DialogResult.OK;
        }

        private void comboBox_Actions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox_Actions.SelectedItem is null)
                _selectedAction = null;
            else if (this.comboBox_Actions.SelectedItem is HotkeyActionController value)
                _selectedAction = value;
            else
                _selectedAction = null;
        }

        private void comboBox_Hotkeys_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.comboBox_Hotkeys.SelectedItem is null)
                _selectedHotkey = null;
            else if (this.comboBox_Hotkeys.SelectedItem is ComboHotkeyValue value)
                _selectedHotkey = value.Code;
            else
                _selectedHotkey = null;
        }

        private void listBox_Values_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBox_Values.SelectedItem is null)
                _selectedEntry = null;
            else if (this.listBox_Values.SelectedItem is HotkeyListBoxEntry value)
                _selectedEntry = value;
            else
                _selectedEntry = null;
        }

        #region Static Interface
        private static readonly ConcurrentBag<HotkeyActionController> _actionControllers = new();
        private static readonly IReadOnlyList<UnityKeyCode> _allKeys = Enum.GetValues(typeof(UnityKeyCode)).Cast<UnityKeyCode>().ToList();
        private static ConcurrentDictionary<UnityKeyCode, HotkeyAction> _hotkeys = new();
        private static MainForm _parent;

        /// <summary>
        /// Global App Config.
        /// </summary>
        private static Config Config { get; } = Program.Config;

        /// <summary>
        /// All Hotkeys for this Application.
        /// </summary>
        internal static IReadOnlyDictionary<UnityKeyCode, HotkeyAction> Hotkeys => _hotkeys;

        /// <summary>
        /// Initialization Method called by Main GUI Thread/Window.
        /// MUST ONLY BE CALLED ONCE!
        /// </summary>
        /// <param name="parent">Form Reference to Main Window (Parent).</param>
        internal static void Initialize(MainForm parent)
        {
            if (_parent is not null)
                throw new InvalidOperationException("Hotkey Manager has already been initialized!");
            if (Config.Hotkeys is IReadOnlyDictionary<int, string> hotkeys)
            {
                foreach (var kvp in hotkeys)
                {
                    var action = new HotkeyAction(kvp.Value);
                    _hotkeys.TryAdd((UnityKeyCode)kvp.Key, action);
                }
            }
            _parent = parent;
        }

        /// <summary>
        /// Register an action controller.
        /// </summary>
        /// <param name="controller">Controller to register.</param>
        internal static void RegisterActionController(HotkeyActionController controller)
        {
            _actionControllers.Add(controller);
        }
        #endregion

        #region Types

        /// <summary>
        /// Wraps a Unity Hotkey/Event Delegate, and maintains it's State.
        /// *NOT* Thread Safe!
        /// Does not need to implement IDisposable (Timer) since this object will live for the lifetime
        /// of the application.
        /// </summary>
        public sealed class HotkeyActionController
        {
            /// <summary>
            /// Action Name used for lookup.
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Delay (ms) between 'HotkeyDelayElapsed' Event Firing.
            /// Default: 100ms
            /// </summary>
            public double Delay
            {
                get => _timer.Interval;
                set => _timer.Interval = value;
            }
            /// <summary>
            /// GUI Thread/Window to execute delegate(s) on.
            /// </summary>
            private MainForm Window { get; set; }
            /// <summary>
            /// Event Occurs when associated Hotkey changes state.
            /// </summary>
            public event EventHandler<HotkeyEventArgs> HotkeyStateChanged = null;
            /// <summary>
            /// Event Occurs during Initial 'Key Down', and repeats while key is down.
            /// Be sure to set the 'Delay' Property (Default: 100ms).
            /// </summary>
            public event EventHandler HotkeyDelayElapsed = null;

            private readonly System.Timers.Timer _timer;
            private bool _state = false;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="name">Name of action.</param>
            /// Required for OnHotkeyDelay.</param>
            public HotkeyActionController(string name)
            {
                Name = name;
                _timer = new()
                {
                    Interval = 100,
                    AutoReset = true
                };
                _timer.Elapsed += OnHotkeyDelayElapsed;
            }

            /// <summary>
            /// Execute the Action.
            /// </summary>
            /// <param name="isKeyDown">True if Hotkey is currently down.</param>
            public void Execute(bool isKeyDown)
            {
                Window ??= _parent;
                bool keyDown = !_state && isKeyDown;
                bool keyUp = _state && !isKeyDown;
                if (keyDown || keyUp) // State has changed
                {
                    UpdateState(keyDown);
                    if (HotkeyStateChanged is not null) // Invoke Event if Set.
                        OnHotkeyStateChanged(new HotkeyEventArgs(keyDown));
                }
            }

            /// <summary>
            /// Executed whenever a Hotkey Changes State.
            /// Updates the Internal 'State' of this controller and it's Events.
            /// </summary>
            /// <param name="newState">New State of the Hotkey.
            /// True: Key is down.
            /// False: Key is up.</param>
            private void UpdateState(bool newState)
            {
                _state = newState;
                if (HotkeyDelayElapsed is not null) // Set 'HotkeyDelayElapsed' State
                {
                    if (newState) // Key Down
                    {
                        Window?.BeginInvoke(() =>
                        {
                            HotkeyDelayElapsed?.Invoke(this, EventArgs.Empty); // Invoke Delay Event on Initial Keydown
                        });
                        _timer.Start(); // Start Callback Timer
                    }
                    else // Key Up
                        _timer.Stop(); // Stop Timer (Resets to 0)
                }
            }

            /// <summary>
            /// Invokes 'HotkeyStateChanged' Event Delegate.
            /// </summary>
            private void OnHotkeyStateChanged(HotkeyEventArgs e)
            {
                Window?.BeginInvoke(() =>
                {
                    HotkeyStateChanged?.Invoke(this, e);
                });
            }

            /// <summary>
            /// Invokes 'HotkeyDelayElapsed' Event Delegate.
            /// </summary>
            private void OnHotkeyDelayElapsed(object sender, System.Timers.ElapsedEventArgs e)
            {
                Window?.BeginInvoke(() =>
                {
                    HotkeyDelayElapsed?.Invoke(this, EventArgs.Empty);
                });
            }

            public override string ToString() => Name;

            public sealed class HotkeyEventArgs : EventArgs
            {
                /// <summary>
                /// State of the Hotkey.
                /// True: Key is down.
                /// False: Key is up.
                /// </summary>
                public bool State { get; }

                public HotkeyEventArgs(bool state) : base()
                {
                    State = state;
                }
            }
        }

        /// <summary>
        /// Links a Unity Hotkey to it's Action Controller.
        /// Wrapper for GUI/Backend Interop.
        /// </summary>
        public sealed class HotkeyAction
        {
            /// <summary>
            /// Action Name used for lookup.
            /// </summary>
            public string Name { get; }
            /// <summary>
            /// Action Controller to execute.
            /// </summary>
            private HotkeyActionController Action { get; set; }

            public HotkeyAction(string name)
            {
                Name = name;
            }

            /// <summary>
            /// Execute the Hotkey action controller.
            /// </summary>
            /// <param name="isKeyDown">True if the key is pressed.</param>
            public void Execute(bool isKeyDown)
            {
                Action ??= _actionControllers.FirstOrDefault(x => x.Name == Name);
                Action?.Execute(isKeyDown);
            }

            public override string ToString() => Name;
        }
        /// <summary>
        /// ListBox wrapper for Hotkey/Action Entries in Hotkey Manager.
        /// </summary>
        public sealed class HotkeyListBoxEntry
        {
            private readonly string _name;
            /// <summary>
            /// Hotkey Key Value.
            /// </summary>
            public UnityKeyCode Hotkey { get; }
            /// <summary>
            /// Hotkey Action Object that contains state/delegate.
            /// </summary>
            public HotkeyAction Action { get; }

            public HotkeyListBoxEntry(UnityKeyCode hotkey, HotkeyAction action)
            {
                Hotkey = hotkey;
                Action = action;
                _name = hotkey.GetDescription();
            }

            public override string ToString() => $"{Action.Name} == {_name}";
        }
        /// <summary>
        /// Combo Box Wrapper for UnityKeyCode Enums for Hotkey Manager.
        /// </summary>
        public sealed class ComboHotkeyValue
        {
            /// <summary>
            /// Full name of the Key.
            /// </summary>
            public string Key { get; }
            /// <summary>
            /// Key enum value.
            /// </summary>
            public UnityKeyCode Code { get; }

            public ComboHotkeyValue(UnityKeyCode keyCode)
            {
                Key = keyCode.GetDescription();
                Code = keyCode;
            }

            public override string ToString() => Key;
        }

        #endregion
    }
}
