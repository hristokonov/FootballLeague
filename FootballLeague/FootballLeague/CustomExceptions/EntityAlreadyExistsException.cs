using System;

namespace FootballLeague.CustomExceptions
{
    public class EntityAlreadyExistsException : Exception
    {
        public EntityAlreadyExistsException()
        {

        }
        public EntityAlreadyExistsException(string message) : base(message)
        {

        }
    }
}
