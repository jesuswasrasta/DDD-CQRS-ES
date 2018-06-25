﻿using System;
using Autofac;
using FourSolid.Cqrs.OrdiniClienti.Domain.CommandsHandler;
using FourSolid.Cqrs.OrdiniClienti.Messages.Commands;
using Paramore.Brighter;
using Polly;

namespace FourSolid.Cqrs.OrdiniClienti.Mediator
{
    public class InstantiateCommandProcessor : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(context =>
                {
                    #region Policies
                    var retryPolicy = Policy.Handle<Exception>().WaitAndRetry(new[]
                    {
                        TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150)
                    });
                    var circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreaker(1, TimeSpan.FromMilliseconds(500));
                    var retryPolicyAsync = Policy.Handle<Exception>().WaitAndRetryAsync(new[]
                    {
                        TimeSpan.FromMilliseconds(50), TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(150)
                    });
                    var circuitBreakerPolicyAsync = Policy.Handle<Exception>().CircuitBreakerAsync(1, TimeSpan.FromMilliseconds(500));
                    var policyRegistry = new PolicyRegistry
                    {
                        { CommandProcessor.RETRYPOLICY, retryPolicy },
                        { CommandProcessor.CIRCUITBREAKER, circuitBreakerPolicy },
                        { CommandProcessor.RETRYPOLICYASYNC, retryPolicyAsync },
                        { CommandProcessor.CIRCUITBREAKERASYNC, circuitBreakerPolicyAsync }
                    };
                    #endregion

                    var subscriberRegistry = new SubscriberRegistry();
                    subscriberRegistry.RegisterAsync<CreateOrdineCliente, CreateOrdineClienteCommandHandler>();
                    

                    var servicesHandlerFactory = context.Resolve<IAmAHandlerFactoryAsync>();

                    return CommandProcessorBuilder.With()
                        .Handlers(new HandlerConfiguration(subscriberRegistry, servicesHandlerFactory))
                        .Policies(policyRegistry)
                        .NoTaskQueues()
                        .RequestContextFactory(new InMemoryRequestContextFactory())
                        .Build();
                }
            ).As<IAmACommandProcessor>().SingleInstance();
        }
    }
}