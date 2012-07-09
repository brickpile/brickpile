#How BrickPile Works
So you've installed BrickPile, now what?
##The Tour: BrickPile's Database
BrickPile is built upon the very popular document database called [RavenDB](http://ravendb.net).

Data in RavenDB is stored schema-less as JSON documents, and can be queried efficiently using Linq queries from .NET code or using RESTful API.
In BrickPile you have the full power of RavenDB at your fingers, to read up on how to use RavenDB, head over to the [RavenDB documentation](http://ravendb.net/docs)

##Page Type Models
A page type model in BrickPile describes which content the page should have. For example a standard page will most certainly have different content then a form page.
Let's look at an example

	[PageType(Name = "Standard page", ControllerType = typeof(PageController))]
    public class StandardPage : PageModel {
		public string Heading { get; set; }
		[DataType(DataType.MultilineText)]
		public string MainIntro { get; set; }
		[DataType(DataType.Html)]
		public string MainBody { get; set; }
	}

The above example defines a page type named Standard page, the controller handling all pages of this type is called **PageController**.
This page type have three properties, all strings in this case but can of course be more complex objects.

**Note:** If you have complex objects in your page type model you most certainly need to create a custom editor template for it, more on that in the Add-on Development section.

##The CurrentPage
The current page is essential when working with BrickPile and there is a couple of ways to get hold of it.

### Constructor Injected Access
In this example the Current Page is injected into the constructor, strongly typed and then returned to the view.

	public class PageController : Controller {
		private readonly StandardPage _currentPage;
		public ActionResult Index() {
			return View(_currentPage);
		}
		public PageController(StandardPage currentPage) {
			_currentPage = currentPage;
		}
	}

The markup in the view would be something like this

	@model StandardPage
	@{
		Layout = "~/Views/Shared/_Layout.cshtml";
	}
	<h1>@Model.Heading</h1>
	<p class="introduction">
		@Model.MainIntro
	</p>
	@Html.Raw(Model.MainBody)

Minimalistic and clean!

### Paramter Based Access
Instead of getting the current page injected into the constructor you can have it handed to you as an action method paramter called **currentPage**.

Let's look at an example

	public class PageController : Controller {
		public ActionResult Index(StandardPage currentPage) {
			return View(currentPage);
		}
	}

###Advanced Scenarios
If you wish to handle the Current Page yourself you can use

	ControllerContext.RequestContext.RouteData.GetCurrentPage<StandardPage>

or maybe

	ControllerContext.RequestContext.RouteData.GetCurrentPage<IPageModel>

##Automatic Navigation
In BrickPile it's very easy to get the navigation up'n running using built in HTML-helpers and the **StructureInfo** object.

###The StructureInfo
The StructureInfo object is injected into your controllers constructor and contains all expanded pages sorted by the default sort order, which is visible in menu and published.

	public class PageController : Controller {
		private IStructureInfo _structureInfo;
		public ActionResult Index(StandardPage currentPage) {
			var viewModel = new DefaultViewModel<StandardPage> {
				CurrentPage = currentPage,
				Pages = _structureInfo.Pages 
			}
			return View(viewModel);
		}
		public PageController(IStructureInfo structureInfo) {
			_structureInfo = structureInfo;
		}
	}

As you can see we made a slight change to our controller by getting the StructureInfo and creating a view model base on the built in DefaultViewModel<T>, you can of course create your own view model which is recommended.

###Main Menu Helper

Now we can add the navigation to our `_Layout.cshtml`

	@model BrickPile.UI.Web.ViewModels.IViewModel<StandardPage>
	@using BrickPile.UI.Web.Mvc.Html
	@using BrickPile.UI.Common

	<!DOCTYPE html>
	<html>
		<head><title></title></head>
		<body>
			<header>
				<nav>
					@Html.Menu(Model.Pages,
						page => Html.ActionLink(page))        
				</nav>
			</header>
		</body>
	</html>

Yes, it's that easy! The Menu helper has a couple more paramters like SelectedItemContent and ExpandedItemContent, you can easily add different markup to each state like this

	@Html.Menu(Model.Pages,
		page => Html.ActionLink(page),
		page => Html.DisplayFor(x => page.Metadata.Name, "SelectedTemplate"),
		page => Html.ActionLink(page, new { @class = "expanded" }))

In both of these scenarios we are using the built in ActionLink helper that takes an **IPageModel** as a parameter, you can of course use the standard ActionLink helper but then it's important the add a custom route value called currentPage like this `page => Html.ActionLink(page.Metadata.Name,"index", new { currentPage = page })`

**Note:** The above examples is also used if you wish to link to another page inline without the menu helper or maybe post a form.

###Sub Menu Helper
In most cases if you are building an information heavy web site you also need a sub menu, an hierarchical structured menu that makes it possible for the visitors to drill down in the information structure.
Creating such a menu is just as easy as creating the main menu

	@Html.SubMenu(Model.Pages,
		page => Html.ActionLink(page),
		page => Html.DisplayFor(x => page.Metadata.Name, "SelectedTemplate"),
		page => Html.ActionLink(page, new { @class = "expanded" }))

**Thats it!** It can't be easier then this, right?