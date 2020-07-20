namespace DataAccessLibrary
{
    /// <summary>
    ///   <para>Indicates to the server the state of an entity about the operation that should be performed for a related entity.</para>
    /// </summary>
    public enum EntityState
    {
        Unchanged,
        Created,
        Changed
    }
}