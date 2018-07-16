using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace CondorExtreme3_API.Helper
{
    public static class FrameworkExceptionHandler
    {
        public enum ExceptionType
        {
            PhoneUniquenessViolation,
            UsernameUniquenessViolation,
            EmailUniquenessViolation,
            BankAccountNumberUniquenessViolation,
            EmployeeEmploymentsDuplicate,
            VirtualPointsPacketsUniquenessAmountViolation

        }

        public static Dictionary<ExceptionType,string> ExceptionsMapper {
            get {
                return new Dictionary<ExceptionType, string>
                {
                   {
                    ExceptionType.PhoneUniquenessViolation,
                    "Specified phone number already exists. Please enter a new value!"                   
                   }
                   ,
                   {
                    ExceptionType.EmailUniquenessViolation, 
                   "Specified email address already exists.Please enter a new value!"      
                   },
                   {
                    ExceptionType.UsernameUniquenessViolation,
                    "Specified username is already taken!"
                   },
                   {
                    ExceptionType.BankAccountNumberUniquenessViolation,
                    "Specified bank account number already exists!"
                   },
                   {
                    ExceptionType.EmployeeEmploymentsDuplicate,
                    "Chosen employee already has specified employment!"
                   },
                    {
                    ExceptionType.VirtualPointsPacketsUniquenessAmountViolation,
                    "There already exists Virtual Points packet with that amount!"
                   }

                };
            }
        }

    }
}