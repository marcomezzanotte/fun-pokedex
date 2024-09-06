namespace FunPokedex.Core.Errors
{
    /// <summary>
    /// Enumerations of all domain error codes
    /// </summary>
    public static class DomainErrors
    {
        public static readonly DomainError UnknownPokemon = new DomainError("No pokemon found with given name", new DomainErrorCode("pokemon/not-found"), DomainErrorStatuses.NotFound);
        public static readonly DomainError CannotReadPokemonData = new DomainError("An issue occured reading Pokemon data", new DomainErrorCode("pokemon/failed-reading-data"), DomainErrorStatuses.DependencyError);
        public static readonly DomainError CannotTranslatePokemonData = new DomainError("An issue occured translating Pokemon data", new DomainErrorCode("pokemon/failed-translating-data"), DomainErrorStatuses.DependencyError);
    }
}
