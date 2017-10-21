using apistation.owin.Commands;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace apistation.owin.Depends
{
    public class DefaultRouter : IRouter
    {
        private IDictionary<String, Type> _get;
        private IDictionary<String, Type> _put;
        private IDictionary<String, Type> _post;
        private IDictionary<String, Type> _delete;
        private readonly ICommand defaultCommand;

        public DefaultRouter()
        {
            _get = new Dictionary<String, Type>();
            _put = new Dictionary<String, Type>();
            _post = new Dictionary<String, Type>();
            _delete = new Dictionary<String, Type>();

            defaultCommand = new StatusGetCommand(new DefaultCache());

            // scan for other commands
            StartCommandScan();
        }

        public ICommand Route(IOwinRequest request)
        {
            ICommand command = new DefaultCommand();
            switch (request.Method)
            {
                case "GET":
                    command = SelectCommand(_get, request.Path.Value, typeof(DefaultGetCommand));
                    break;

                case "POST":
                    command = SelectCommand(_post, request.Path.Value, typeof(DefaultPostCommand));
                    break;

                case "PUT":
                    command = SelectCommand(_put, request.Path.Value, typeof(DefaultPutCommand));
                    break;

                case "DELETE":
                    command = SelectCommand(_delete, request.Path.Value, typeof(DefaultDeleteCommand));
                    break;

                default:
                    break;
            }

            return command;
        }

        private void StartCommandScan()
        {
            Type[] commands = ObjectScanner.Scan<ICommand, CommandOptionsAttribute>();
            foreach (var command in commands)
            {
                var commandOptions = command.GetCustomAttributes<CommandOptionsAttribute>();
                if (commandOptions.Any())
                {
                    switch (commandOptions.First().Method.ToLower())
                    {
                        case "get": // need type constraints
                            _get.Add(commandOptions.First().PathExpression, command);
                            break;

                        case "post": // need type constraints
                            _post.Add(commandOptions.First().PathExpression, command);
                            break;

                        case "put": // need type constraints
                            _put.Add(commandOptions.First().PathExpression, command);
                            break;

                        case "delete":// need type constraints
                            _delete.Add(commandOptions.First().PathExpression, command);
                            break;
                    }
                }
            }
        }

        private ICommand SelectCommand(IDictionary<string, Type> _routeDictionary, string routeExpression, Type commandType)
        {
            var matchExpression = routeExpression.ToLower();

            // match algorithm
            var matches = _routeDictionary
                                    .Where(t => t.Key == "/*" || t.Key.ToLower().StartsWith(matchExpression));

            // match algorithm results
            if (matches.Any())
            {
                var match = matches.First();
                return (ICommand)Activator.CreateInstance(match.Value, ObjectFactory.Resolve<ICache>());
            }

            return (ICommand)Activator.CreateInstance(defaultCommand.GetType(), ObjectFactory.Resolve<ICache>());
        }
    }
}