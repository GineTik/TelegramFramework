﻿using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Telegramper.Attributes.TargetExecutorAttributes;
using Telegramper.Executors.Configuration.Options;
using Telegramper.Executors.Helpers.Factories.Executors;
using Telegramper.Executors.Routing;
using Telegramper.Executors.Routing.ParametersParser;
using Telegramper.Executors.Routing.Storage;
using Telegramper.Executors.Routing.Storage.RouteDictionaries;
using Telegramper.Executors.Routing.Storage.StaticHelpers;
using Telegramper.Executors.Storages.Command;
using Telegramper.Executors.Storages.UserState;
using Telegramper.Executors.Storages.UserState.Saver;

namespace Telegramper.Executors.Configuration.Services
{
    public static class ExecutorExtensions
    {
        public static IServiceCollection AddExecutors(this IServiceCollection services,
            Action<ExecutorOptions>? configure = null)
        {
            return services.AddExecutors(null, configure);
        }

        public static IServiceCollection AddExecutors(this IServiceCollection services,
            Assembly[]? assemblies = null, Action<ExecutorOptions>? configure = null)
        {
            var executorsTypes = getExecutorsTypes(assemblies);
            var executorOptions = configureOptions(services, executorsTypes, configure);

            services.addTransientServices(executorsTypes, executorOptions);
            services.addSingletonServices(executorsTypes, executorOptions);

            return services;
        }

        private static IEnumerable<Type> getExecutorsTypes(Assembly[]? assemblies = null)
        {
            assemblies ??= new[] { Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly() };

            var baseExecutorType = typeof(Executor);
            var executorsTypes = assemblies.SelectMany(assembly =>
                assembly.GetTypes().Where(type => type != baseExecutorType && baseExecutorType.IsAssignableFrom(type))
            );

            return executorsTypes;
        }

        private static ExecutorOptions configureOptions(IServiceCollection services,
            IEnumerable<Type> executorsTypes, Action<ExecutorOptions>? configure = null)
        {
            var executorOptions = new ExecutorOptions();
            configure?.Invoke(executorOptions);

            services.Configure<ParameterParserOptions>(options =>
            {
                options.DefaultSeparator = executorOptions.ParameterParser.DefaultSeparator;
                options.ParserType = executorOptions.ParameterParser.ParserType;
                options.ErrorMessages = executorOptions.ParameterParser.ErrorMessages;
            });

            services.Configure<UserStateOptions>(options =>
            {
                options.DefaultUserState = executorOptions.UserState.DefaultUserState;
                options.SaverType = executorOptions.UserState.SaverType;
            });

            services.Configure<TargetMethodOptinons>(options => 
            {
                options.ExecutorsTypes = executorsTypes;
                options.MethodInfos = ExecutorMethodsHelper.TakeExecutorMethodsFrom(executorsTypes);
            });

            return executorOptions;
        }

        private static void addTransientServices(this IServiceCollection services, IEnumerable<Type> executorsTypes,
            ExecutorOptions executorOptions)
        {
            services.AddTransient<IExecutorRouter, ExecutorRouter>();
            services.AddTransient<IExecutorFactory, ExecutorFactory>();
            services.AddTransient(typeof(IParametersParser), executorOptions.ParameterParser.ParserType);

            foreach (var type in executorsTypes)
                services.AddTransient(type);
        }

        private static void addSingletonServices(this IServiceCollection services, IEnumerable<Type> executorsTypes,
            ExecutorOptions executorOptions)
        {
            var routesStorage = createRoutesStorage(executorsTypes, executorOptions);
            var commandStorage = createCommandStorage(routesStorage.Methods);
            
            services.AddSingleton<ICommandStorage, ExecutorCommandStorage>(_ => commandStorage);
            services.AddSingleton<IRoutesStorage, RoutesStorage>(_ => routesStorage);
            
            services.AddSingleton<IUserStateStorage, UserStateStorage>();
            services.AddSingleton(typeof(IUserStateSaver), executorOptions.UserState.SaverType);
        }

        private static ExecutorCommandStorage createCommandStorage(IEnumerable<MethodInfo> methods)
        {
            var commandAttributes = methods
                .SelectMany(method => method.GetCustomAttributes<TargetCommandsAttribute>());

            return new ExecutorCommandStorage(commandAttributes);
        }

        private static RoutesStorage createRoutesStorage(IEnumerable<Type> executorsTypes, ExecutorOptions executorOptions)
        {
            var methods = ExecutorMethodsHelper.TakeExecutorMethodsFrom(executorsTypes);
            var routes = new UpdateTypeDictionary(executorOptions.UserState.DefaultUserState);
            routes.AddMethods(methods);

            return new RoutesStorage(methods, routes);
        }
    }
}