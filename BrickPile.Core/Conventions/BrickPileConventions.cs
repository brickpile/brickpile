namespace BrickPile.Core.Conventions
{
    public class BrickPileConventions
    {
        /// <summary>
        /// Gets or sets the virtual path provider conventions.
        /// </summary>
        /// <value>
        /// The virtual path provider conventions.
        /// </value>
        public VirtualPathProviderConventions VirtualPathProviderConventions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrickPileConventions" /> class.
        /// </summary>
        public BrickPileConventions()
        {
            VirtualPathProviderConventions = new VirtualPathProviderConventions();
        }
    }
}
