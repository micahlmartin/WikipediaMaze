using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace WikipediaMaze.Core
{
    public class OpenIdProfile
    {
        /// <summary>
        /// The user's OpenID URL. Use this value to sign the user in to your website. This field is always present.
        /// </summary>
        public string Identifier { get; set; }

        /// <summary>
        /// A human-readable name of the authentication provider that was used for this authentication. For well-known providers, RPX sends values such as "Google", "Facebook", and "MySpace"; "Other" is sent for other providers. New provider names are added over time.
        /// </summary>
        public string ProviderName { get; set; }

        /// <summary>
        /// Primary key of the user in your database. Only present if you are using the mappings API.
        /// </summary>
        public string PrimaryKey { get; set; }

        /// <summary>
        /// The name of this Contact, suitable for display to end-users.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The preferred username of this contact on sites that ask for a username.
        /// </summary>
        public string PreferredUserName { get; set; }

        /// <summary>
        /// A dictionary of name parts. See the name field section for details.
        /// </summary>
        public Name Name { get; set; }

        /// <summary>
        /// The gender of this person. Canonical values are 'male', and 'female', but may be any value
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Date of birth in YYYY-MM-DD format. Year field may be 0000 if unavailable.
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// The offset from UTC of this Contact's current time zone, as of the time this response was returned. The value MUST conform to the offset portion of xs:dateTime, e.g. -08:00. Note that this value MAY change over time due to daylight saving time, and is thus meant to signify only the current value of the user's timezone offset.
        /// </summary>
        public string UtcOffset { get; set; }

        /// <summary>
        /// An email address at which the person may be reached.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// An email address at which the person may be reached.
        /// </summary>
        public string VerifiedEmail { get; set; }

        /// <summary>
        /// URL of a webpage relating to this person.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// A phone number at which the person may be reached.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// URL to a photo (GIF/JPG/PNG) of the person.
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Contains all the parts of the address.
        /// </summary>
        public Address Address { get; set; }
    }

}
