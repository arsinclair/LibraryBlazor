namespace DataAccessLibrary
{
    /// <summary>
    ///     <para>Provides a collection of attributes for an entity.</para>
    /// </summary>
    /// <remarks>
    ///     <para>For each attribute in the collection there is a key/value pair. The key is the logical name of the attribute as defined in the attribute metadata. </para>
    /// </remarks>
    public sealed class AttributeCollection : DataCollection<string, object>
    {
    }
}