using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FinancePlanner.TaxServices.Application.Models.Exceptions;
using FinancePlanner.TaxServices.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace FinancePlanner.TaxServices.Application.PluginHandler
{
    public class PluginFactory : IPluginFactory
    {
        private readonly Dictionary<string, Assembly> _pluginMap;
        private readonly Dictionary<string, string> _pluginConfig;
        private readonly IConfiguration _configuration;
        private readonly IFederalTaxBracketRepository _federalTaxBracketRepository;

        public PluginFactory(IConfiguration configuration, IFederalTaxBracketRepository federalTaxBracketRepository)
        {
            _pluginMap = new Dictionary<string, Assembly>();
            _configuration = configuration;
            _federalTaxBracketRepository = federalTaxBracketRepository;
            _pluginConfig = _configuration.GetSection("W4PluginConfig")
                .GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);
        }

        public static Assembly LoadPlugin(string assemblyFileName)
        {
            string solutionRoot = Path.GetFullPath(Path.GetDirectoryName(typeof(PluginFactory).Assembly.Location)
                                                   ?? throw new ApplicationException("Something went wrong while getting assembly path!"));
            string pluginLocation = Path.GetFullPath(Path.Combine(solutionRoot, $"plugins\\{Path.GetFileNameWithoutExtension(assemblyFileName)}",
                assemblyFileName.Replace('\\', Path.DirectorySeparatorChar)));
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        public T GetService<T>(string localeType)
        {
            try
            {
                _pluginMap.TryGetValue(localeType, out Assembly assembly);
                Type pluginType = assembly?.GetTypes().FirstOrDefault(c => typeof(T).IsAssignableFrom(c));

                if (pluginType == null)
                    throw new InternalServerErrorException($"No plugin implementing the interface {nameof(T)} and with name " +
                                                $"{localeType} found in {assembly} at {assembly?.Location}");

                var pluginService = (T)Activator.CreateInstance(pluginType, _configuration, _federalTaxBracketRepository);
                return pluginService;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return default;
            }
        }

        public void Initialize()
        {
            foreach (var plugin in _pluginConfig)
            {
                var (pluginKey, pluginFile) = plugin;
                var assembly = LoadPlugin(pluginFile);
                _pluginMap[pluginKey] = assembly;
            }
        }
    }
}
