using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contacts.Application.Constants
{
    public static class ErrorMessages
    {
        public const string NOT_MAPPED = "Entity could not ne mapped!";
        
        public const string COLLECTION_EMPTY = "Has not been found any record in database!";
        public const string NOT_FOUND_IN_COLLECTION = "Contact has not been found in database!";
        
        public const string ADD_ERROR = "An error occurred while adding data!";
        public const string UPDATE_ERROR = "An error occurred while updating data!";
        public const string DELETE_ERROR = "An error occurred while deleting data!";
    }
}
