﻿namespace API.Exceptions
{
    public class GenreNotFoundException : VodApiException
    {
        public GenreNotFoundException(string genreName) : base($"Genre {genreName} was not found.")
        {

        }
    }
}
