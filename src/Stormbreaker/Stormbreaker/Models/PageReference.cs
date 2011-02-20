using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Stormbreaker.Repositories;
using StructureMap;

namespace Stormbreaker.Models {
    public class PageReference {
        private readonly IPageRepository _pageRepository;
        /// <summary>
        /// Gets or sets the selected page model.
        /// </summary>
        /// <value>
        /// The selected page model.
        /// </value>
        [ScaffoldColumn(false)]
        public string SelectedPageModel { get; set; }
        /// <summary>
        /// Gets the page model.
        /// </summary>
        [ScaffoldColumn(false)]
        [JsonIgnore]
        public IPageModel PageModel
        {
            get
            {
                if (!string.IsNullOrEmpty(SelectedPageModel))
                {
                    return _pageRepository.Load<IPageModel>(SelectedPageModel);
                }
                return null;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="PageReference"/> class.
        /// </summary>
        public PageReference()
        {
            _pageRepository = ObjectFactory.GetInstance<IPageRepository>();
        }
    }
}