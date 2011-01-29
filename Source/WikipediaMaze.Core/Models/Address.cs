using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    public class Address
    {
        /// <summary>
        /// The full mailing address, formatted for display or use with a mailing label.
        /// </summary>
        public string Formatted { get; set; }

        /// <summary>
        /// The full street address component, which may include house number, street name, PO BOX, and multi-line extended street address information.
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// The city or locality component.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// The state or region component.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Postal code or zipcode.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// The country name component.
        /// </summary>
        public string Country { get; set; }
    }
}
