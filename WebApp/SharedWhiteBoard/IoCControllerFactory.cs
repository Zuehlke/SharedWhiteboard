using System;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

namespace SharedWhiteBoard
{
    public class IoCControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public IoCControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                return _container.Resolve(controllerType) as IController;
            }

            return base.GetControllerInstance(requestContext, null);
        }
    }
}