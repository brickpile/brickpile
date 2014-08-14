using System.Linq;
using BrickPile.Core;
using BrickPile.Samples.Models.ContentParts;
using BrickPile.Samples.Models.ContentTypes;
using Raven.Client.Indexes;

namespace BrickPile.Samples.Business
{
    public class HomeViewModelTransformer : AbstractTransformerCreationTask<Page>
    {
        public HomeViewModelTransformer()
        {
            TransformResults = pages => from page in pages

                let model = LoadDocument<Home>(page.Id)
                let features = model.Features
                let services = model.ServicesCarousel
                let portfolio = model.PortfolioCarousel
                let team = model.TeamCarousel

                select new
                {
                    CurrentPage = model,
                    Hero = LoadDocument<Hero>(model.Hero.Id),
                    FeaturesHeading = features.Heading,
                    Features = LoadDocument<Feature>(features.FeatureList.Select(x => x.Id)),
                    Services = LoadDocument<Service>(services.Select(x => x.Id)),
                    Portfolio = LoadDocument<PortfolioItem>(portfolio.Select(x => x.Id)),
                    Team = LoadDocument<TeamMember>(team.Select(x => x.Id))
                };
        }
    }
}