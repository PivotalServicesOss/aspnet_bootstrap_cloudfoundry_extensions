
namespace PivotalServices.CloudFoundry.Replatform.Bootstrap.Base
{
    public interface IActuator
    {
        void Configure();
        void Stop();
        void Start();
    }
}
