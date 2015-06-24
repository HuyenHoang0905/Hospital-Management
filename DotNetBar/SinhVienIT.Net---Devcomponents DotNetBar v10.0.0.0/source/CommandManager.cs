using System;
using System.Text;
using System.Collections;

namespace DevComponents.DotNetBar
{
    /// <summary>
    /// Provides command related utility methods that register and unregister commands.
    /// </summary>
    public class CommandManager
    {
        private static Hashtable _CommandBindings = new Hashtable();
        private static bool _UseReflection = true;

        /// <summary>
        /// Gets or sets whether commands use Reflection to find the property names when applying value to the unknown types. Default value is true.
        /// Setting this value to false will increase performance if Unknown types are used but it will at same time disable the
        /// command setting value for these types.
        /// </summary>
        public static bool UseReflection
        {
            get
            {
                return _UseReflection;
            }
            set
            {
                _UseReflection = value;
            }
        }

        private static bool _AutoUpdateLayout = true;
        /// <summary>
        /// Gets or sets whether layout on the items of type BaseItem is automatically updated when command text or other property changes which requires layout updated. Default value is true.
        /// </summary>
        public static bool AutoUpdateLayout
        {
            get { return _AutoUpdateLayout; }
            set
            {
                if (_AutoUpdateLayout != value)
                {
                    _AutoUpdateLayout = value;
                }
            }
        }

        /// <summary>
        /// Connects the Command Source to the Command.
        /// </summary>
        /// <param name="commandSource">Command source to connect to the command.</param>
        /// <param name="command">Reference to the command.</param>
        public static void RegisterCommand(ICommandSource commandSource, ICommand command)
        {
            if (commandSource == null)
                throw new NullReferenceException("commandSource cannot be null");
            if (command == null)
                throw new NullReferenceException("command cannot be null");

            ArrayList subscribers = null;
            if (_CommandBindings.Contains(command))
            {
                subscribers = (ArrayList)_CommandBindings[command];
                if (!subscribers.Contains(commandSource))
                    subscribers.Add(commandSource);
            }
            else
            {
                subscribers = new ArrayList();
                subscribers.Add(commandSource);
                _CommandBindings.Add(command, subscribers);
            }
            command.CommandSourceRegistered(commandSource);
        }

        /// <summary>
        /// Disconnects command source from the command.
        /// </summary>
        /// <param name="commandSource">Reference to command source.</param>
        /// <param name="command">Reference to the command.</param>
        public static void UnRegisterCommandSource(ICommandSource commandSource, ICommand command)
        {
            if (commandSource == null)
                throw new NullReferenceException("commandSource cannot be null");
            if (command == null)
                throw new NullReferenceException("command cannot be null");
            if (_CommandBindings.Contains(command))
            {
                ArrayList subscribers = (ArrayList)_CommandBindings[command];
                if (subscribers.Contains(commandSource))
                    subscribers.Remove(commandSource);
            }
            command.CommandSourceUnregistered(commandSource);
        }

        /// <summary>
        /// Unregister command from all subscribers. Called when command is disposed.
        /// </summary>
        /// <param name="command">Command to unregister.</param>
        public static void UnRegisterCommand(ICommand command)
        {
            if (command == null)
                throw new NullReferenceException("command cannot be null");

            if (_CommandBindings.Contains(command))
                _CommandBindings.Remove(command);
        }

        /// <summary>
        /// Gets an array of Command Sources that are connected with the command. 
        /// </summary>
        /// <param name="command">Reference to command</param>
        /// <returns>An array of command sources.</returns>
        public static ArrayList GetSubscribers(ICommand command)
        {
            if (command == null)
                throw new NullReferenceException("command cannot be null");
            ArrayList subscribers = null;
            if (_CommandBindings.Contains(command))
                subscribers = (ArrayList)((ArrayList)_CommandBindings[command]).Clone();
            else
                subscribers = new ArrayList();

            return subscribers;
        }

        internal static void ExecuteCommand(ICommandSource commandSource)
        {
            if (commandSource == null)
                throw new NullReferenceException("commandSource cannot be null");
            ICommand command=commandSource.Command;
            if(command == null)
                throw new NullReferenceException("commandSource.Command cannot be null");

            command.Execute(commandSource);
        }
    }
}
