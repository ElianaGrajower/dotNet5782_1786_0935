using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    namespace DO
    {
        namespace Exceptions //not sure about this......
        {
            //class AddingException : Exception
            //{
            //    public AddingException() : base() { }
            //    public AddingException(string message) : base(message) { }
            //    public AddingException(string message,Exception inner) : base(message,inner) { }

            //}
      
            /// <summary>
            /// ///////////////////
            /// </summary>
            //public class InfoTwoStationsMissException : Exception
            //{
            //    public int FirstStation;
            //    public int SecondStation;
            //    public InfoTwoStationsMissException(int station1, int station2) : base()
            //    {
            //        FirstStation = station1;
            //        SecondStation = station2;
            //    }
            //    public InfoTwoStationsMissException(int station1, int station2, string message) : base(message)
            //    {
            //        FirstStation = station1;
            //        SecondStation = station2;
            //    }
            //    public InfoTwoStationsMissException(int station1, int station2, string message, Exception innerException) : base(message, innerException)
            //    {
            //        FirstStation = station1;
            //        SecondStation = station2;
            //    }
            //    public override string ToString() => base.ToString() + $", miss information between stations: {FirstStation} and {SecondStation}";
            //}

            //[Serializable]
            //public class DoesntExistException : Exception
            //{
            //    public DoesntExistException() : base() { }
            //    public DoesntExistException(string message) : base(message) { }
            //    public DoesntExistException(string message, Exception inner) : base(message, inner) { }
            //    protected DoesntExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

            //}

            //public class AlreadyExistException : Exception
            //{
            //    public AlreadyExistException() : base() { }
            //    public AlreadyExistException(string message) : base(message) { }
            //    public AlreadyExistException(string message, Exception inner) : base(message, inner) { }
            //    protected AlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }

            //}

            //public class InvalidInputException : Exception
            //{
            //    public InvalidInputException() : base() { }
            //    public InvalidInputException(string message) : base(message) { }
            //    public InvalidInputException(string message, Exception inner) : base(message, inner) { }
            //    protected InvalidInputException(SerializationInfo info, StreamingContext context) : base(info, context) { }

            //}

            //public class LoadingException : Exception
            //{
            //    string filePath;
            //    public LoadingException() : base() { }
            //    public LoadingException(string message) : base(message) { }
            //    public LoadingException(string message, Exception inner) : base(message, inner) { }

            //    public LoadingException(string path, string messege, Exception inner) => filePath = path;
            //    protected LoadingException(SerializationInfo info, StreamingContext context) : base(info, context) { }

            //}
            [Serializable]
            public class UpdateProblemException : Exception
            {
                public UpdateProblemException() : base() { }
                public UpdateProblemException(string message) : base(message) { }
                public UpdateProblemException(string message, Exception inner) : base(message, inner) { }
                protected UpdateProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
            }

            [Serializable]
            public class GetDetailsProblemException : Exception
            {
                public GetDetailsProblemException() : base() { }
                public GetDetailsProblemException(string message) : base(message) { }
                public GetDetailsProblemException(string message, Exception inner) : base(message, inner) { }
                protected GetDetailsProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
            }

            [Serializable]
            public class DeletedProblemException : Exception
            {
                public DeletedProblemException() : base() { }
                public DeletedProblemException(string message) : base(message) { }
                public DeletedProblemException(string message, Exception inner) : base(message, inner) { }
                protected DeletedProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
            }

            [Serializable]
            public class AddingProblemException : Exception
            {
                public AddingProblemException() : base() { }
                public AddingProblemException(string message) : base(message) { }
                public AddingProblemException(string message, Exception inner) : base(message, inner) { }
                protected AddingProblemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
            }

            [Serializable]
            public class MissDataOfTwoStationsExceptions : Exception
            {
                public int FirstStation;
                public int SecondStation;
                public MissDataOfTwoStationsExceptions(int station1, int station2) : base()
                {
                    FirstStation = station1;
                    SecondStation = station2;
                }
                public MissDataOfTwoStationsExceptions(int station1, int station2, string message) : base(message)
                {
                    FirstStation = station1;
                    SecondStation = station2;
                }
                public MissDataOfTwoStationsExceptions(int station1, int station2, string message, Exception innerException) : base(message, innerException)
                {
                    FirstStation = station1;
                    SecondStation = station2;
                }
                public override string ToString() => base.ToString() + $", miss information between stations: {FirstStation} and {SecondStation}";
            }

            [Serializable]
            public class InvalidValueException : Exception
            {
                public InvalidValueException() : base() { }
                public InvalidValueException(string message) : base(message) { }
                public InvalidValueException(string message, Exception inner) : base(message, inner) { }
                protected InvalidValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
            }

        }
    }
}
