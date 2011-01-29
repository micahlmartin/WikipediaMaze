using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WikipediaMaze.Core
{
    /// <summary>
    /// The components of the person's real name. 
    /// Providers MAY return just the full name as a single string in the formatted sub-field,
    /// or they MAY return just the individual component fields using the other sub-fields, 
    /// or they MAY return both. If both variants are returned, 
    /// they SHOULD be describing the same name, 
    /// with the formatted name indicating how the component fields should be combined.
    /// </summary>
    public class Name
    {
        /// <summary>
        /// The full name, including all middle names, titles, and suffixes as appropriate, formatted for display (e.g. Mr. Joseph Robert Smarr, Esq.). This is the Primary Sub-Field for this field, for the purposes of sorting and filtering.
        /// </summary>
        public string FormattedName { get; set; }

        /// <summary>
        /// The family name of this Contact, or "Last Name" in most Western languages (e.g. Smarr given the full name Mr. Joseph Robert Smarr, Esq.).
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// The given name of this Contact, or "First Name" in most Western languages (e.g. Joseph given the full name Mr. Joseph Robert Smarr, Esq.).
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// The middle name(s) of this Contact (e.g. Robert given the full name Mr. Joseph Robert Smarr, Esq.).
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The honorific prefix(es) of this Contact, or "Title" in most Western languages (e.g. Mr. given the full name Mr. Joseph Robert Smarr, Esq.).
        /// </summary>
        public string HonorificPrefix { get; set; }

        /// <summary>
        /// The honorific suffix(es) of this Contact, or "Suffix" in most Western languages (e.g. Esq. given the full name Mr. Joseph Robert Smarr, Esq.).
        /// </summary>
        public string HonorificSuffix { get; set; }
    }
}
