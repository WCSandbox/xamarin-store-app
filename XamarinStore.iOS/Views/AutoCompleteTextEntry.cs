using MonoTouch.UIKit;
using System.Collections.Generic;
using System.Linq;

namespace XamarinStore
{
	public class AutoCompleteTextEntry : TextEntryView
	{
	    readonly StringTableViewController controller;
		public string Title { get; set; }
		public AutoCompleteTextEntry ()
		{
			controller = new StringTableViewController () {
				ItemSelected = (item) => {
					Value = item;
				}
			};
			

			textField.Started += delegate { Search(); };
		}

		IEnumerable<string> items = new List<string>();

		public IEnumerable<string> Items {
			get {
				return items;
			}
			set {
				items = value;
			}
		}

		void Search ()
		{
			if (!items.Any())
				return;

			textField.ResignFirstResponder();
			controller.Title = Title;
			controller.Items = items;
			if(PresenterView.NavigationController.TopViewController != controller)
				PresenterView.NavigationController.PushViewController (controller, true);


		}

		public UIViewController PresenterView {get;set;}
	}
}

