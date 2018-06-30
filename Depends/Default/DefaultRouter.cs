using apistation.owin.Commands;
using apistation.owin.Support;
using LightInject;
using Microsoft.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSwag;
using NSwag.Collections;
using NJsonSchema;

namespace apistation.owin.Depends
{
    public class DefaultCommandRouter : IRouter
    {
        private static IServiceContainer CommandContainer;
        private ILog _log;
        private IDictionary<String, Type> _get;
        private IDictionary<String, Type> _put;
        private IDictionary<String, Type> _post;
        private IDictionary<String, Type> _delete;

        // swagger defined types
        private IDictionary<String, Type> _routes;

        private readonly ICommand defaultCommand;

        private JObject _model = null;
        private IEnumerable<JToken> _paths;

        public DefaultCommandRouter(ILog log)
        {
            _log = log;
            _routes = new Dictionary<string, Type>();
            _get = new Dictionary<String, Type>();
            _put = new Dictionary<String, Type>();
            _post = new Dictionary<String, Type>();
            _delete = new Dictionary<String, Type>();
            defaultCommand = new StatusGetCommand(new DefaultCache());
            StartCommandScan();
        }

        public ICommand Route(IOwinRequest request)
        {
            ICommand command = new DefaultCommand();

            bool valid = this.validateRequest(request);

            // fail safe - for now if any non valid route is found handle normally
            if (valid)
            {
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
            }

            return command;
        }

        /// <summary>
        /// Find all Commands in the Assembly
        /// </summary>
        private void StartCommandScan()
        {
            /// locate the types
            bool deepScan = true;
            Type[] commands = TypeTree.Scan(typeof(ICommand), deepScan);

            /// register the types in some graph
            foreach (var command in commands)
            {
                var commandOptions = command.GetCustomAttributes<CommandOptionsAttribute>();
                if (commandOptions.Any())
                {
                    switch (commandOptions.First().Method.ToLower())
                    {
                        case "get": // need query syntax (odata)
                            _get.Add(commandOptions.First().PathExpression, command);
                            break;
                        case "post": // need type constraints ?
                            _post.Add(commandOptions.First().PathExpression, command);
                            break;
                        case "put": // need type constraints ?
                            _put.Add(commandOptions.First().PathExpression, command);
                            break;
                        case "delete":  // need type constraints ?
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
            var matches = _routeDictionary.Where(t => t.Key == "/*" || t.Key.ToLower().StartsWith(matchExpression));

            // match algorithm results
            if (matches.Any())
            {
                var match = matches.OrderByDescending(r => r.Key).First();
                return (ICommand)Activator.CreateInstance(match.Value, ApiStartup.Container.Create<ICache>());
            }

            return (ICommand)Activator.CreateInstance(defaultCommand.GetType(), ApiStartup.Container.Create<ICache>());
        }

        private bool validateRequest(IOwinRequest request)
        {
            //return this._paths.Any(p => p.ToString().StartsWith(request.Path.Value));
            return this._routes.Any(r => r.Key.StartsWith(request.Path.Value));
        }

        public void Build(JObject model)
        {
            try
            {
                if (model != null)
                {
                    var modelDoc = NSwag.SwaggerDocument.FromJsonAsync(model.ToString()).Result;
                    _model = model;
                    foreach(var path in modelDoc.Paths)
                    {
                        Console.WriteLine("{0}", path.Key);
                        this._routes.Add(path.Key, typeof(JObject));
                    }
                }
            }
            catch (Newtonsoft.Json.JsonException jsonError)
            {
                _log.Log(jsonError);
            }
        }
    }
}