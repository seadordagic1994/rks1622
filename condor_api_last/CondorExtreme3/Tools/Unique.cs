using System;
using System.Linq;
using System.Globalization;
using System.ComponentModel.DataAnnotations;
using CondorExtreme3.DAL;
using System.Web;

/*-----------------------------------+
 * Author: Ilhan Karic               |
 * Module: Users                     |
 * Last Modified: 1/10/2018 @ 19:36  |
 * Copyright: Condor Coorporation    |
 *-----------------------------------*/
namespace CondorExtreme3.Tools
{
    // Custom data annotation that lets you to check if the attribute is unique in the DB
    public class Unique : ValidationAttribute
    {
        private Type type;
        public Unique(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }
        public Unique(string errorMessage, Type value)
        {
            this.ErrorMessage = errorMessage;
            this.type = value;
        }

        // Kill me please..
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CondorDBContextChild db = new CondorDBContextChild(HttpContext.Current.Session["ConnectionString"].ToString());

            var className = this.type.Name + "s";
            var propertyName = validationContext.MemberName;
            var parameterName = string.Format("@{0}", propertyName);
            
            // Kill me please..
            var result = db.Database.SqlQuery<int>(
                string.Format("SELECT COUNT(*) FROM {0} WHERE {1}={2}", className, propertyName, parameterName),
                new System.Data.SqlClient.SqlParameter(parameterName, value));

            // Kill me please..
            if (result.ToList()[0] > 0)
                return new ValidationResult(string.Format(this.ErrorMessage));
            
            // End my suffering
            return null;
        }

        // Fancy text formatting
        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentCulture,
              ErrorMessageString, name);
        }
    }
}