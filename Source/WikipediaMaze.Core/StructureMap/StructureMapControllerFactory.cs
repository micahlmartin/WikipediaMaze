using System;
using System.Web.Mvc;
using StructureMap;

namespace WikipediaMaze.Core.StructureMap
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                return base.GetControllerInstance(requestContext, controllerType);

            return ObjectFactory.GetInstance(controllerType) as IController;
        }
    }
}


