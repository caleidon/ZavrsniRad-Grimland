// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Local

using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace CommandTerminal
{
    public static class BuiltinCommands
    {
        [RegisterCommand(Name = "help", Help = "Display help information about a command", MaxArgCount = 1)]
        private static void CommandHelp(CommandArg[] args)
        {
            if (args.Length == 0)
            {
                foreach (var command in Terminal.Shell.Commands)
                {
                    Terminal.Log($"{command.Key.PadRight(16)}: {command.Value.Help}");
                }

                return;
            }

            string commandName = args[0].String.ToUpper();

            if (!Terminal.Shell.Commands.TryGetValue(commandName, out CommandInfo info))
            {
                Terminal.Shell.IssueErrorMessage($"Command {commandName} could not be found.");
                return;
            }

            if (info.Help == null)
            {
                Terminal.Log($"{commandName} does not provide any help documentation.");
            }
            else if (info.Hint == null)
            {
                Terminal.Log(info.Help);
            }
            else
            {
                Terminal.Log($"{info.Help}\nUsage: {info.Hint}");
            }
        }

        [RegisterCommand(Name = "tick", Help = "Displays current map tick", MaxArgCount = 0)]
        private static void CommandTick(CommandArg[] args)
        {
            Terminal.Log(TickManager.Tick.ToString());
        }

        [RegisterCommand(Name = "clear", Help = "Clear the console", MaxArgCount = 0)]
        private static void CommandClear(CommandArg[] args)
        {
            Terminal.Buffer.Clear();
        }

        [RegisterCommand(Name = "time", Help = "Time the execution of a command", MinArgCount = 1, Hint = "time <command>")]
        private static void CommandTime(CommandArg[] args)
        {
            var sw = new Stopwatch();
            sw.Start();

            Terminal.Shell.RunCommand(JoinArguments(args));

            sw.Stop();
            Terminal.Log($"Time: {(double)sw.ElapsedTicks / 10000}ms");
        }

        [RegisterCommand(Name = "reloaddefs", Help = "Deletes all cached defs and reloads them", MinArgCount = 0)]
        private static void CommandReloadDefs(CommandArg[] args)
        {
            DefManager.ReloadDefs();
            Terminal.Log("Defs reloaded successfully");
        }

        [RegisterCommand(Name = "region_info", Help = "Logs information about the number of regions, rooms and links", MaxArgCount = 0)]
        private static void CommandRegionInfo(CommandArg[] args)
        {
            Terminal.Log($"Regions: {RegionManager.Regions.Count}, Rooms: {RoomManager.Rooms.Count}, Links: {RegionManager.Links.Count}");
        }

        [RegisterCommand(Name = "add", Help = "Adds 2 numbers", MinArgCount = 2, MaxArgCount = 2)]
        private static void CommandAdd(CommandArg[] args)
        {
            int a = args[0].Int;
            int b = args[1].Int;

            if (Terminal.IssuedError) return; // Error will be handled by Terminal

            int result = a + b;
            Terminal.Log("{0} + {1} = {2}", a, b, result);
        }

        [RegisterCommand(Name = "spawn", Help = "Spawns an entity at the cursor position", MinArgCount = 1, MaxArgCount = 1, Hint = "spawn <entityName>")]
        private static void CommandSpawn(CommandArg[] args)
        {
            string entity = args[0].String;

            if (Terminal.IssuedError) return;

            Vector3Int gridPosition = Map.GridPosFromMousePos();
            if (!Map.Contains(gridPosition)) return;

            // TODO implement creature spawning in terminal

            // if (DefManager.Creatures.TryGetValue(entity, out Creature.Initializer creature))
            // {
            //     creature.Create(gridPosition);
            //     Terminal.Log($"Spawned {entity} at position {gridPosition}");
            // }
            // else if (DefManager.VegetationTileInstances.TryGetValue(entity, out VegetationTileDef vegetation)) { Map.Instance.SetTile(gridPosition, vegetation, Map.TilemapType.VegetationTilemap); }
            // else { Terminal.Log("Entity not found."); }
        }

        private static string JoinArguments(CommandArg[] args, int start = 0)
        {
            var sb = new StringBuilder();
            int argLength = args.Length;

            for (int i = start; i < argLength; i++)
            {
                sb.Append(args[i].String);

                if (i < argLength - 1)
                {
                    sb.Append(" ");
                }
            }

            return sb.ToString();
        }
    }
}