using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace CommandTerminal
{
    public enum TerminalState
    {
        Close,
        Open
    }

    public class Terminal : MonoBehaviour
    {
        [Header("Window")] [Range(0, 1)] [SerializeField]
        private float maxHeight = 0.3f;

        [SerializeField] private float toggleSpeed = 1000;

        [SerializeField] private string toggleHotkey = "`";
        [SerializeField] private int bufferSize = 512;

        [Header("Input")] [SerializeField] private Font consoleFont;
        [SerializeField] private string inputCaret = ">";

        [SerializeField] private Color backgroundColor = Color.black;
        [SerializeField] private Color foregroundColor = Color.white;
        [SerializeField] private Color shellColor = Color.white;
        [SerializeField] private Color inputColor = Color.cyan;
        [SerializeField] private Color warningColor = Color.yellow;
        [SerializeField] private Color errorColor = Color.red;

        private static TerminalState state;
        private TextEditor editorState;
        private bool inputFix;
        private bool moveCursor;
        private Rect window;
        private static float currentOpenT;
        private static float openTarget;
        private float realWindowSize;
        private string commandText;
        private string cachedCommandText;
        private Vector2 scrollPosition;
        private GUIStyle windowStyle;
        private GUIStyle labelStyle;
        private GUIStyle inputStyle;
        private Texture2D backgroundTexture;
        private Texture2D inputBackgroundTexture;

        public static CommandLog Buffer { get; private set; }
        public static CommandShell Shell { get; private set; }
        private static CommandHistory History { get; set; }
        private static CommandAutocomplete Autocomplete { get; set; }

        internal static bool IssuedError => Shell.IssuedErrorMessage != null;

        public static bool IsClosed => state == TerminalState.Close && Mathf.Approximately(currentOpenT, openTarget);

        public static void Log(string format, params object[] message)
        {
            Log(TerminalLogType.ShellMessage, format, message);
        }

        private static void Log(TerminalLogType type, string format, params object[] message)
        {
            Buffer.HandleLog(string.Format(format, message), type);
        }

        private void SetState(TerminalState newState)
        {
            inputFix = true;
            cachedCommandText = commandText;
            commandText = "";

            switch (newState)
            {
                case TerminalState.Close:
                {
                    openTarget = 0;
                    break;
                }
                case TerminalState.Open:
                {
                    openTarget = Screen.height * maxHeight;
                    if (currentOpenT > openTarget)
                    {
                        openTarget = 0;
                        state = TerminalState.Close;
                        return;
                    }

                    realWindowSize = openTarget;
                    scrollPosition.y = int.MaxValue;
                    break;
                }
                default:
                {
                    realWindowSize = Screen.height * maxHeight;
                    openTarget = realWindowSize;
                    break;
                }
            }

            state = newState;
        }

        private void ToggleState(TerminalState newState)
        {
            SetState(state == newState ? TerminalState.Close : newState);
        }

        private void OnEnable()
        {
            Buffer = new CommandLog(bufferSize);
            Shell = new CommandShell();
            History = new CommandHistory();
            Autocomplete = new CommandAutocomplete();

            // Hook Unity log events
            Application.logMessageReceivedThreaded += HandleUnityLog;
        }

        private void OnDisable()
        {
            Application.logMessageReceivedThreaded -= HandleUnityLog;
        }

        private void Start()
        {
            if (consoleFont == null)
            {
                consoleFont = Font.CreateDynamicFontFromOSFont("Courier New", 16);
                Debug.LogWarning("Command Console Warning: Please assign a font.");
            }

            commandText = "";
            cachedCommandText = commandText;
            Assert.AreNotEqual(toggleHotkey.ToLower(), "return", "Return is not a valid ToggleHotkey");

            SetupWindow();
            SetupInput();
            SetupLabels();

            Shell.RegisterCommands();

            if (IssuedError)
            {
                Log(TerminalLogType.Error, "Error: {0}", Shell.IssuedErrorMessage);
            }

            foreach (var command in Shell.Commands)
            {
                Autocomplete.Register(command.Key);
            }
        }

        private void OnGUI()
        {
            if (Event.current.Equals(Event.KeyboardEvent(toggleHotkey)))
            {
                SetState(TerminalState.Open);
            }

            if (IsClosed)
            {
                return;
            }

            HandleOpenness();
            window = GUILayout.Window(88, window, DrawConsole, "", windowStyle);
        }

        private void SetupWindow()
        {
            realWindowSize = Screen.height * maxHeight / 3;
            window = new Rect(0, currentOpenT - realWindowSize, Screen.width, realWindowSize);

            // Set background color
            backgroundTexture = new Texture2D(1, 1);
            backgroundTexture.SetPixel(0, 0, backgroundColor);
            backgroundTexture.Apply();

            windowStyle = new GUIStyle { normal = { background = backgroundTexture }, padding = new RectOffset(4, 4, 4, 4) };
            windowStyle.normal.textColor = foregroundColor;
            windowStyle.font = consoleFont;
        }

        private void SetupLabels()
        {
            labelStyle = new GUIStyle { font = consoleFont, normal = { textColor = foregroundColor }, wordWrap = true };
        }

        private void SetupInput()
        {
            inputStyle = new GUIStyle { padding = new RectOffset(4, 4, 4, 4), font = consoleFont, fixedHeight = consoleFont.fontSize * 1.6f, normal = { textColor = inputColor } };

            var darkBackground = new Color { r = backgroundColor.r, g = backgroundColor.g, b = backgroundColor.b };

            inputBackgroundTexture = new Texture2D(1, 1);
            inputBackgroundTexture.SetPixel(0, 0, darkBackground);
            inputBackgroundTexture.Apply();
            inputStyle.normal.background = inputBackgroundTexture;
        }

        private void DrawConsole(int window2D)
        {
            GUILayout.BeginVertical();

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, false, GUIStyle.none, GUIStyle.none);
            GUILayout.FlexibleSpace();
            DrawLogs();
            GUILayout.EndScrollView();

            if (moveCursor)
            {
                CursorToEnd();
                moveCursor = false;
            }

            if (Event.current.Equals(Event.KeyboardEvent("escape")))
            {
                SetState(TerminalState.Close);
            }
            else if (Event.current.Equals(Event.KeyboardEvent("return"))
                     || Event.current.Equals(Event.KeyboardEvent("[enter]")))
            {
                EnterCommand();
            }
            else if (Event.current.Equals(Event.KeyboardEvent("up")))
            {
                commandText = History.Previous();
                moveCursor = true;
            }
            else if (Event.current.Equals(Event.KeyboardEvent("down")))
            {
                commandText = History.Next();
            }
            else if (Event.current.Equals(Event.KeyboardEvent(toggleHotkey)))
            {
                ToggleState(TerminalState.Open);
            }
            else if (Event.current.Equals(Event.KeyboardEvent("tab")))
            {
                CompleteCommand();
                moveCursor = true; // Wait till next draw call
            }

            GUILayout.BeginHorizontal();

            if (inputCaret != "")
            {
                GUILayout.Label(inputCaret, inputStyle, GUILayout.Width(consoleFont.fontSize));
            }

            GUI.SetNextControlName("command_text_field");
            commandText = GUILayout.TextField(commandText, inputStyle);

            if (inputFix && commandText.Length > 0)
            {
                commandText = cachedCommandText; // Otherwise the TextField picks up the ToggleHotkey character event
                inputFix = false; // Prevents checking string Length every draw call
            }

            if (!IsClosed)
            {
                GUI.FocusControl("command_text_field");
            }

            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        private void DrawLogs()
        {
            foreach (var log in Buffer.Logs)
            {
                labelStyle.normal.textColor = GetLogColor(log.Type);
                GUILayout.Label(log.Message, labelStyle);
            }
        }

        private void HandleOpenness()
        {
            float dt = toggleSpeed * Time.unscaledDeltaTime;

            if (currentOpenT < openTarget)
            {
                currentOpenT += dt;
                if (currentOpenT > openTarget) currentOpenT = openTarget;
            }
            else if (currentOpenT > openTarget)
            {
                currentOpenT -= dt;
                if (currentOpenT < openTarget) currentOpenT = openTarget;
            }
            else
            {
                if (inputFix)
                {
                    inputFix = false;
                }

                return; // Already at target
            }

            window = new Rect(0, currentOpenT - realWindowSize, Screen.width, realWindowSize);
        }

        private void EnterCommand()
        {
            Log(TerminalLogType.Input, "{0}", commandText);
            Shell.RunCommand(commandText);
            History.Push(commandText);

            if (IssuedError)
            {
                Log(TerminalLogType.Error, "Error: {0}", Shell.IssuedErrorMessage);
            }

            commandText = "";
            scrollPosition.y = int.MaxValue;
        }

        private void CompleteCommand()
        {
            string headText = commandText;
            int formatWidth = 0;

            string[] completionBuffer = Autocomplete.Complete(ref headText, ref formatWidth);
            int completionLength = completionBuffer.Length;

            if (completionLength != 0)
            {
                commandText = headText;
            }

            if (completionLength > 1)
            {
                // Print possible completions
                var logBuffer = new StringBuilder();

                foreach (string completion in completionBuffer)
                {
                    logBuffer.Append(completion.PadRight(formatWidth + 4));
                }

                Log("{0}", logBuffer);
                scrollPosition.y = int.MaxValue;
            }
        }

        private void CursorToEnd()
        {
            if (editorState == null)
            {
                editorState = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
            }

            editorState.MoveCursorToPosition(new Vector2(999, 999));
        }

        private void HandleUnityLog(string message, string stackTrace, LogType type)
        {
            Buffer.HandleLog(message, stackTrace, (TerminalLogType)type);
            scrollPosition.y = int.MaxValue;
        }

        private Color GetLogColor(TerminalLogType type)
        {
            switch (type)
            {
                case TerminalLogType.Message: return foregroundColor;
                case TerminalLogType.Warning: return warningColor;
                case TerminalLogType.Input: return inputColor;
                case TerminalLogType.ShellMessage: return shellColor;
                default: return errorColor;
            }
        }
    }
}