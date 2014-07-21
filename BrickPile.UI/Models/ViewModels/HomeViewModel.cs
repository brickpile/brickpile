using System.Collections.Generic;
using BrickPile.Core;
using BrickPile.UI.Web.ViewModels;

namespace BrickPile.Samples.Models.ViewModels
{
    public class HomeViewModel : IViewModel<Home>
    {
        public Home CurrentPage { get; set; }

        public IEnumerable<IPage> NavigationContext { get; set; }
        
    }
}