using System;

namespace FootballLeague.CustomExceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException()
        {
                
        }
        public EntityNotFoundException(string message) : base(message)
        {

        }
    }
}
