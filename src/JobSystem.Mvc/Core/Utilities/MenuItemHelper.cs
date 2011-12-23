﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Mvc.Html;

public static class HtmlExtensions {
	public static MvcHtmlString MenuLink(
		this HtmlHelper htmlHelper,
		string linkText,
		string actionName,
		string controllerName,
		string cssClass
	)
	{
		string currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
		string currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
		if (actionName == currentAction && controllerName == currentController)
		{
			return htmlHelper.ActionLink(
				linkText,
				actionName,
				controllerName,
				null,
				new
				{
					@class = "active " + cssClass
				});
		}
		return htmlHelper.ActionLink(linkText, actionName, controllerName);
	}
}