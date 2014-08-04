namespace BrickPile.Core.Conventions
{
    /// <summary>
    ///     BrickPile configurable conventions
    /// </summary>
    public class BrickPileConventions
    {
        /// <summary>
        ///     Gets or sets the virtual path provider conventions.
        /// </summary>
        /// <value>
        ///     The virtual path provider conventions.
        /// </value>
        public VirtualPathProviderConventions VirtualPathProviderConventions { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BrickPileConventions" /> class.
        /// </summary>
        public BrickPileConventions()
        {
            this.VirtualPathProviderConventions = new VirtualPathProviderConventions();
        }
    }
}