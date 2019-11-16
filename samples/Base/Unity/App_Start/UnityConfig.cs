using System;
using Unity;
using UnitySample.Services;

namespace UnitySample
{
    public static class UnityConfig
    {
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;

        public static void RegisterTypes(IUnityContainer container)
        {
            // TODO: Register your type's mappings here.
            container.RegisterType<ICalcService, CalcService>();
        }
    }
}