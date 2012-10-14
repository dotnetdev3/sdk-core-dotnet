using System;
using System.Collections.Generic;
using System.Text;

namespace PayPal.Authentication
{
    public class SubjectAuthorization : IThirdPartyAuthorization
    {
        /// <summary>
        /// Subject information
        /// </summary>
        private string subjct;
           
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="subject"></param>
        public SubjectAuthorization(string subject) : base()
        {
            if (string.IsNullOrEmpty(subject))
            {
                throw new ArgumentException("SubjectAuthorization arguments cannot be null or empty");
            }
            this.subjct = subject;
        }

        /// <summary>
        /// Gets the subject
        /// </summary>
        public string Subject
        {
            get
            {
                return subjct;
            }
        }
    }
}
