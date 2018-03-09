using System.Web.Mvc;
using BrickPile.Core;
using BrickPile.Core.Mvc;
using BrickPile.Samples.Models.ContentTypes;
using BrickPile.Samples.Models.ViewModels;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System.Linq;
using System.Threading.Tasks;
using Raven.Client.Documents.Queries;
using BrickPile.Samples.Models.ContentParts;

namespace BrickPile.Samples.Controllers
{
    [AllowAnonymous, EditorControls]
    public class HomeController : Controller
    {
        private readonly IDocumentStore documentStore;

        public HomeController(IDocumentStore documentStore, INavigationContext navigationContext, IBrickPileContext context)
        {
            this.documentStore = documentStore;
        }

        public async Task<ActionResult> Index(Page currentPage, Home currentContent)
        {
            using (IDocumentSession session = documentStore.OpenSession())
            {
                var query = from page in session.Query<Home>()
                            where page.Id == currentPage.Id                            
                            let featuresHeading = page.Features.Heading
                            let features = RavenQuery.Load<Feature>(page.Features.FeatureList.Select(x => x.Id))
                            let services = RavenQuery.Load<Service>(page.ServicesCarousel.Select(x => x.Id))
                            let portfolio = RavenQuery.Load<PortfolioItem>(page.PortfolioCarousel.Select(x => x.Id))
                            let team = RavenQuery.Load<TeamMember>(page.TeamCarousel.Select(x => x.Id))
                            let hero = session.Load<Hero>(page.Hero.Id)
                            select new
                            {
                                CurrentPage = page,
                                Hero = hero,
                                FeaturesHeading = featuresHeading,
                                Features = features,
                                Services = services,
                                Portfolio = portfolio,
                                Team = team
                            };

                var viewModel = query.FirstOrDefault();

                return View(new HomeViewModel {
                    CurrentPage = viewModel.CurrentPage,
                    Hero = viewModel.Hero,
                    FeaturesHeading = viewModel.FeaturesHeading,
                    Features = viewModel.Features.ToList(),
                    Services = viewModel.Services.ToList(),
                    Team = viewModel.Team.ToList()
                });
            }
        }
    }
}