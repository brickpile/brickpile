#Create a custom property for tagging content
In this example, I will show you how to create a custom property using an editor template.

Please note that this post is in no way an attempt to illustrate best practices for ASP.NET MVC or any other technology, I just want to show you how easy it is to create a custom property in BrickPile.

I have chosen to create a property used for tagging content.

We start in our model, add the following line of code to one of your page models that you would like to enable tagging on.

	public ICollection<string> Tags { get; set; }

The content editor in BrickPile does not know how to render this type of collection, so we need to tell it which to use by adding the UIHint attribute.

	[UIHint("Tags")]
	public ICollection<string> Tags { get; set; }

Now we need to add the partial view that will render our tags, create a folder called EditorTemplates inside of Views\Shared in **your** project like this

![Solution explorer](images/solution-explorer.png)

and add the following markup.

	@using BrickPile.UI.Common
	@Html.Section(@<script src="@Url.Content("~/scripts/jquery.tagsinput.min.js")" type="text/javascript"></script>, "scripts")
	@Html.Section(@<link rel="stylesheet" type="text/css" href="@Url.Content("~/content/jquery.tagsinput.css")" />, "styles")

From our partial view, that is used as an editor template we need to add external scripts and css files. To do that, use the extension method in BrickPile.UI.Common named Section on the HtmlHelper. This is pretty cool way of adding javascript and css files to the edit view. As you can see I have used the plugin [jQuery tags input by XOXCO](http://xoxco.com/clickable/jquery-tags-input), download the package and put them in the correct place.

	@{
		var tags = Model != null ? string.Join(",", Model) : "";
	}
	<script type="text/javascript">
		$(function () {
			$('#@ViewData.TemplateInfo.GetFullHtmlFieldId(string.Empty)').tagsInput({
				width: 588
			});
		})
	</script>
	<input type="text" id="@ViewData.TemplateInfo.GetFullHtmlFieldId(string.Empty)" name="@ViewData.TemplateInfo.HtmlFieldPrefix" value="@tags" />

The above code should also be placed inside the the editor template, the lines of javascript should maybe be placed in an external file, but then again, this is a sample of how you can create a custom property in BrickPile.

If you compile and run this code and edit a page of the type you just added tags to you should see something like this

![Tags property](images/tags-property.png)

Awesome, right? … But, we’re not ready yet. If you add some tags here and save the page, the model binder will bind the values as a comma separated string instead of a list of strings. So for this to work we need to create a custom model binder.

Add a new class with the following content

     public class TagsModelBinder : DefaultModelBinder {
        protected override void BindProperty(ControllerContext controllerContext, ModelBindingContext bindingContext, System.ComponentModel.PropertyDescriptor propertyDescriptor) {
            if (propertyDescriptor.PropertyType == typeof(ICollection<string>)) {
                var form = controllerContext.HttpContext.Request.Form;
                var tagsAsString = form["CurrentModel.Tags"];
                var model = bindingContext.Model as Page;
                model.Tags = string.IsNullOrEmpty(tagsAsString)
                    ? new List<string>()
                    : tagsAsString.Split(',').Select(i => i.Trim()).ToList();
            }
            base.BindProperty(controllerContext, bindingContext, propertyDescriptor);
        }
    }

This model binder above can be very improved but for this tutorial I think it will do the job. As a final step we should tell our model to use this model binder when saving a page. Add the model binder attribute to the page model you added the tags property to. In my case the final page model looks like this.

    [ModelBinder(typeof(TagsModelBinder))]
    public class Page : PageModel {
        [UIHint("Tags")]
        public ICollection<string> Tags { get; set; }
    }

Now, if you try to save the page with a couple of tags set you will see that it’s getting saved as a list and not a comma separated string. In RavenDB management studio it should look something like this.

 ![Tags saved](images/tags-saved-in-ravendb.png)

 Now when we can tag our content we can actually use it to create a cloud or whatever, see the [tutorial on ravendb.net](http://beta.ravendb.net/kb/2/creating-a-tag-cloud) if you are interested in doing so.