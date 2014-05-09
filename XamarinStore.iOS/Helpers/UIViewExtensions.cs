using MonoTouch.UIKit;

namespace XamarinStore
{
	public static class UIViewExtensions
	{
		public static UIViewController GetParentViewController(this UIView view)
		{
			var nextResponder = view.NextResponder;
		    var responder = nextResponder as UIViewController;
		    if (responder != null)
				return responder;
		    var uiView = nextResponder as UIView;
		    return uiView != null ? GetParentViewController (uiView) : null;
		}
	}
}

