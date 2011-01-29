using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace WikipediaMaze.Core.Mvc
{
    public class CacheResult : ActionResult
    {
        #region Fields

        private string _keyname;
        private string _version;
        private string _type;

        #endregion

        #region Constructors

        public CacheResult(string keyname, string version, string type)
        {
            _keyname = keyname;
            _version = version;

            if (type.ToLower().Contains("css"))
                _type = @"text/css";

            if (type.ToLower().Contains("javascript"))
                _type = @"text/javascript";
        }

        #endregion

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            ScriptCombiner myCombiner = new ScriptCombiner(_keyname, _version, _type);
            myCombiner.ProcessRequest(context.HttpContext);
        }
    }
}
