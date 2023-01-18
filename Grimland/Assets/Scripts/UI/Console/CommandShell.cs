using System;
using System.Collections.Generic;
using System.Reflection;

namespace CommandTerminal
{
    public struct CommandInfo
    {
        public Action<CommandArg[]> Proc;
        public int MAXArgCount;
        public int MINArgCount;
        public string Help;
        public string Hint;
    }

    public struct CommandArg
    {
        public string String { get; set; }

        public int Int
        {
            get
            {
                if (int.TryParse(String, out var intValue))
                {
                    return intValue;
                }

                TypeError("int");
                return 0;
            }
        }

        public float Float
        {
            get
            {
                if (float.TryParse(String, out var floatValue))
                {
                    return floatValue;
                }

                TypeError("float");
                return 0;
            }
        }

        public bool Bool
        {
            get
            {
                if (string.Compare(String, "TRUE", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }

                if (string.Compare(String, "FALSE", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return false;
                }

                TypeError("bool");
                return false;
            }
        }

        public override string ToString()
        {
            return String;
        }

        private void TypeError(string expectedType)
        {
            Terminal.Shell.IssueErrorMessage(
                "Incorrect type for {0}, expected <{1}>",
                String, expectedType
            );
        }
    }

    public class CommandShell
    {
        private readonly List<CommandArg> arguments = new List<CommandArg>(); // Cache for performance

        public string IssuedErrorMessage { get; private set; }

        public Dictionary<string, CommandInfo> Commands { get; } = new Dictionary<string, CommandInfo>();

        private Dictionary<string, CommandArg> Variables { get; } = new Dictionary<string, CommandArg>();

        public void RegisterCommands()
        {
            var rejectedCommands = new Dictionary<string, CommandInfo>();
            const BindingFlags methodFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                foreach (var method in type.GetMethods(methodFlags))
                {
                    var attribute = Attribute.GetCustomAttribute(
                        method, typeof(RegisterCommandAttribute)) as RegisterCommandAttribute;

                    if (attribute == null)
                    {
                        if (method.Name.StartsWith("FRONTCOMMAND", StringComparison.CurrentCultureIgnoreCase))
                        {
                            // Front-end Command methods don't implement RegisterCommand, use default attribute
                            attribute = new RegisterCommandAttribute();
                        }
                        else
                        {
                            continue;
                        }
                    }

                    var methodsParams = method.GetParameters();

                    string commandName = InferFrontCommandName(method.Name);

                    commandName = attribute.Name ?? InferCommandName(commandName ?? method.Name);

                    if (methodsParams.Length != 1 || methodsParams[0].ParameterType != typeof(CommandArg[]))
                    {
                        // Method does not match expected Action signature,
                        // this could be a command that has a FrontCommand method to handle its arguments.
                        rejectedCommands.Add(commandName.ToUpper(), CommandFromParamInfo(methodsParams, attribute.Help));
                        continue;
                    }

                    // Convert MethodInfo to Action.
                    // This is essentially allows us to store a reference to the method,
                    // which makes calling the method significantly more performant than using MethodInfo.Invoke().
                    var proc = (Action<CommandArg[]>)Delegate.CreateDelegate(typeof(Action<CommandArg[]>), method);
                    AddCommand(commandName, proc, attribute.MinArgCount, attribute.MaxArgCount, attribute.Help, attribute.Hint);
                }
            }

            HandleRejectedCommands(rejectedCommands);
        }

        /// <summary>
        ///     Parses an input line into a command and runs that command.
        /// </summary>
        public void RunCommand(string line)
        {
            string remaining = line;
            IssuedErrorMessage = null;
            arguments.Clear();

            while (remaining != "")
            {
                var argument = EatArgument(ref remaining);

                if (argument.String != "")
                {
                    if (argument.String[0] == '$')
                    {
                        string variableName = argument.String.Substring(1).ToUpper();

                        if (Variables.ContainsKey(variableName))
                        {
                            // Replace variable argument if it's defined
                            argument = Variables[variableName];
                        }
                    }

                    arguments.Add(argument);
                }
            }

            if (arguments.Count == 0)
            {
                // Nothing to run
                return;
            }

            string commandName = arguments[0].String.ToUpper();
            arguments.RemoveAt(0); // Remove command name from arguments

            if (!Commands.ContainsKey(commandName))
            {
                IssueErrorMessage("Command {0} could not be found", commandName);
                return;
            }

            RunCommand(commandName, arguments.ToArray());
        }

        private void RunCommand(string commandName, CommandArg[] commandArgs)
        {
            var command = Commands[commandName];
            int argCount = commandArgs.Length;
            string errorMessage = null;
            int requiredArg = 0;

            if (argCount < command.MINArgCount)
            {
                errorMessage = command.MINArgCount == command.MAXArgCount ? "exactly" : "at least";
                requiredArg = command.MINArgCount;
            }
            else if (command.MAXArgCount > -1 && argCount > command.MAXArgCount)
            {
                // Do not check max allowed number of arguments if it is -1
                errorMessage = command.MINArgCount == command.MAXArgCount ? "exactly" : "at most";
                requiredArg = command.MAXArgCount;
            }

            if (errorMessage != null)
            {
                string pluralFix = requiredArg == 1 ? "" : "s";

                IssueErrorMessage(
                    "{0} requires {1} {2} argument{3}",
                    commandName,
                    errorMessage,
                    requiredArg,
                    pluralFix
                );

                if (command.Hint != null)
                {
                    IssuedErrorMessage += $"\n    -> Usage: {command.Hint}";
                }

                return;
            }

            command.Proc(commandArgs);
        }

        private void AddCommand(string name, CommandInfo info)
        {
            name = name.ToUpper();

            if (Commands.ContainsKey(name))
            {
                IssueErrorMessage("Command {0} is already defined.", name);
                return;
            }

            Commands.Add(name, info);
        }

        private void AddCommand(string name, Action<CommandArg[]> proc, int minArgs = 0, int maxArgs = -1, string help = "", string hint = null)
        {
            var info = new CommandInfo
            {
                Proc = proc,
                MINArgCount = minArgs,
                MAXArgCount = maxArgs,
                Help = help,
                Hint = hint
            };

            AddCommand(name, info);
        }

        public void SetVariable(string name, string value)
        {
            SetVariable(name, new CommandArg { String = value });
        }

        private void SetVariable(string name, CommandArg value)
        {
            name = name.ToUpper();

            if (Variables.ContainsKey(name))
            {
                Variables[name] = value;
            }
            else
            {
                Variables.Add(name, value);
            }
        }

        public CommandArg GetVariable(string name)
        {
            name = name.ToUpper();

            if (Variables.ContainsKey(name))
            {
                return Variables[name];
            }

            IssueErrorMessage("No variable named {0}", name);
            return new CommandArg();
        }

        public void IssueErrorMessage(string format, params object[] message)
        {
            IssuedErrorMessage = string.Format(format, message);
        }

        private static string InferCommandName(string methodName)
        {
            int index = methodName.IndexOf("COMMAND", StringComparison.CurrentCultureIgnoreCase);

            var commandName = index >= 0 ? methodName.Remove(index, 7) : methodName;

            return commandName;
        }

        private static string InferFrontCommandName(string methodName)
        {
            int index = methodName.IndexOf("FRONT", StringComparison.CurrentCultureIgnoreCase);
            return index >= 0 ? methodName.Remove(index, 5) : null;
        }

        private void HandleRejectedCommands(Dictionary<string, CommandInfo> rejectedCommands)
        {
            foreach (var command in rejectedCommands)
            {
                if (Commands.ContainsKey(command.Key))
                {
                    Commands[command.Key] = new CommandInfo
                    {
                        Proc = Commands[command.Key].Proc,
                        MINArgCount = command.Value.MINArgCount,
                        MAXArgCount = command.Value.MAXArgCount,
                        Help = command.Value.Help
                    };
                }
                else
                {
                    IssueErrorMessage("{0} is missing a front command.", command);
                }
            }
        }

        private static CommandInfo CommandFromParamInfo(ParameterInfo[] parameters, string help)
        {
            int optionalArgs = 0;

            foreach (var param in parameters)
            {
                if (param.IsOptional)
                {
                    optionalArgs += 1;
                }
            }

            return new CommandInfo
            {
                Proc = null,
                MINArgCount = parameters.Length - optionalArgs,
                MAXArgCount = parameters.Length,
                Help = help
            };
        }

        private static CommandArg EatArgument(ref string s)
        {
            var arg = new CommandArg();
            int spaceIndex = s.IndexOf(' ');

            if (spaceIndex >= 0)
            {
                arg.String = s.Substring(0, spaceIndex);
                s = s.Substring(spaceIndex + 1); // Remaining
            }
            else
            {
                arg.String = s;
                s = "";
            }

            return arg;
        }
    }
}