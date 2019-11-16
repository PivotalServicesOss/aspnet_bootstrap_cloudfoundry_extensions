using Autofac;
using AutofacSample.Services;
using System;

namespace AutofacSample
{
    public class AutofacConfig
    {
        private static Lazy<IContainer> container =
          new Lazy<IContainer>(() =>
          {
              RegisterTypes();
              return Builder.Build();
          });

        public static IContainer Container => container.Value;

        public static ContainerBuilder Builder { get; private set; } = new ContainerBuilder();

        public static void RegisterTypes()
        {
            // TODO: Register your type's mappings here.
            Builder.RegisterType<CalcService>().As<ICalcService>();
        }
    }
}