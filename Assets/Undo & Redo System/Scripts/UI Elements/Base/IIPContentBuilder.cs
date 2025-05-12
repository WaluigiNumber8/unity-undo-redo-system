namespace RedRats.UndoRedo.UIElements
{
    /// <summary>
    /// Interface allowing content builders to delete their content.
    /// </summary>
    public interface IIPContentBuilder
    {
        /// <summary>
        /// Empty contents.
        /// </summary>
        public void Clear();
    }
}