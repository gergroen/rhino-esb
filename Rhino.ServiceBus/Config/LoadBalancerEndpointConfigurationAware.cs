using System;
using System.Configuration;
using Rhino.ServiceBus.Impl;
using Rhino.ServiceBus.MessageModules;
using Rhino.ServiceBus.Internal;

namespace Rhino.ServiceBus.Config
{
    public class LoadBalancerEndpointConfigurationAware : IBusConfigurationAware
    {
        public void Configure(AbstractRhinoServiceBusConfiguration config, IBusContainerBuilder builder, IServiceLocator locator)
        {
            var busConfig = config as RhinoServiceBusConfiguration;
            if (busConfig == null)
                return;

            var busElement = config.ConfigurationSection.Bus;

            if (busElement == null)
                return;

            var loadBalancerEndpointAsString = busElement.LoadBalancerEndpoint;

            if (string.IsNullOrEmpty(loadBalancerEndpointAsString))
                return;

            Uri loadBalancerEndpoint;
            if (!Uri.TryCreate(loadBalancerEndpointAsString, UriKind.Absolute, out loadBalancerEndpoint))
                throw new ConfigurationErrorsException(
                    "Attribute 'loadBalancerEndpoint' on 'bus' has an invalid value '" + loadBalancerEndpointAsString + "'");

            var endpoint = new Endpoint { Uri = loadBalancerEndpoint };
            builder.RegisterLoadBalancerEndpoint(endpoint.Uri);
            config.AddMessageModule<LoadBalancerMessageModule>();
        }
    }
}