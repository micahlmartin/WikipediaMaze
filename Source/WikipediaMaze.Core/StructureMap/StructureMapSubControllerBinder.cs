using System;
using MvcContrib.Binders;
using StructureMap;

namespace WikipediaMaze.Core.StructureMap
{
    public class StructureMapSubControllerBinder : SubControllerBinder
    {
        public override object CreateSubController(Type destinationType)
        {
            var instance = ObjectFactory.GetInstance(destinationType);
            if (instance == null)
            {
                throw new InvalidOperationException(destinationType + " not registered with StructureMap");
            }

            return instance;
        }
    }
}


