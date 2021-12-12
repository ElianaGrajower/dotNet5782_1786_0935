using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    [Serializable]
    public class DoesntExistException : Exception  //if the item doesn't exist
    {
        public DoesntExistException() : base() { }
        public DoesntExistException(string message) : base(message) { }
        public DoesntExistException(string message, Exception inner) : base(message, inner) { }
       

    }

    [Serializable]
    public class AlreadyExistsException : Exception  //if the item already exists
    {
        public AlreadyExistsException() : base() { }
        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        

    }

    
    public class InvalidInputException : Exception  //if the input is invalid
    {
        public InvalidInputException() : base() { }
        public InvalidInputException(string message) : base(message) { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
        

    }
    public class UnableToCompleteRequest : Exception  //if the program is Unable to complete the request 
    {
        public UnableToCompleteRequest() : base() { }
        public UnableToCompleteRequest(string message) : base(message) { }
        public UnableToCompleteRequest(string message, Exception inner) : base(message, inner) { }


    }
    
        public class unavailableException : Exception //if the item is unavailable
    {
        public unavailableException() : base() { }
        public unavailableException(string message) : base(message) { }
        public unavailableException(string message, Exception inner) : base(message, inner) { }


    }


}
