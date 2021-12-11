using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBL.BO
{
    [Serializable]
    public class DoesntExistException : Exception
    {
        public DoesntExistException() : base() { }
        public DoesntExistException(string message) : base(message) { }
        public DoesntExistException(string message, Exception inner) : base(message, inner) { }
       

    }

    [Serializable]
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base() { }
        public AlreadyExistsException(string message) : base(message) { }
        public AlreadyExistsException(string message, Exception inner) : base(message, inner) { }
        

    }

    
    public class InvalidInputException : Exception
    {
        public InvalidInputException() : base() { }
        public InvalidInputException(string message) : base(message) { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
        

    }
    public class UnableToCompleteRequest : Exception
    {
        public UnableToCompleteRequest() : base() { }
        public UnableToCompleteRequest(string message) : base(message) { }
        public UnableToCompleteRequest(string message, Exception inner) : base(message, inner) { }


    }
    
        public class unavailableException : Exception
    {
        public unavailableException() : base() { }
        public unavailableException(string message) : base(message) { }
        public unavailableException(string message, Exception inner) : base(message, inner) { }


    }


}
