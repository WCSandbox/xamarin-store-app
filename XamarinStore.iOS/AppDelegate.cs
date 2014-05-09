using System;
using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace XamarinStore.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public static AppDelegate Shared;

		UIWindow _window;
		UINavigationController _navigation;

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Shared = this;
			FileCache.SaveLocation = System.IO.Directory.GetParent (Environment.GetFolderPath (Environment.SpecialFolder.Personal)) + "/tmp";

			UIApplication.SharedApplication.SetStatusBarStyle (UIStatusBarStyle.LightContent, false);

			_window = new UIWindow (UIScreen.MainScreen.Bounds);
			UINavigationBar.Appearance.SetTitleTextAttributes (new UITextAttributes {
				TextColor = UIColor.White
			});

			var productVc = new ProductListViewController ();
			productVc.ProductTapped += ShowProductDetail;
			_navigation = new UINavigationController (productVc);

			_navigation.NavigationBar.TintColor = UIColor.White;
			_navigation.NavigationBar.BarTintColor = Color.Blue;

			_window.RootViewController = _navigation;
			_window.MakeKeyAndVisible ();
			return true;
		}

		public void ShowProductDetail (Product product)
		{
			var productDetails = new ProductDetailViewController (product);
			productDetails.AddToBasket += p => {
				WebService.Shared.CurrentOrder.Add (p);
				UpdateProductsCount();
			};
			_navigation.PushViewController (productDetails, true);
		}
		public void ShowBasket ()
		{
			var basketVc = new BasketViewController (WebService.Shared.CurrentOrder);
			basketVc.Checkout += (sender, e) => ShowLogin ();
			_navigation.PushViewController (basketVc, true);
		}

		public void ShowLogin ()
		{
			var loginVc = new LoginViewController ();
			loginVc.LoginSucceeded += ShowAddress;
			_navigation.PushViewController (loginVc, true);
		}

		public void ShowAddress ()
		{
			var addreesVc = new ShippingAddressViewController (WebService.Shared.CurrentUser);
			addreesVc.ShippingComplete += (sender, e) => ProccessOrder ();
			_navigation.PushViewController (addreesVc, true);
		}

		public void ProccessOrder()
		{
			var processing = new ProcessingViewController (WebService.Shared.CurrentUser);
			processing.OrderPlaced += (sender, e) => OrderCompleted ();
			_navigation.PresentViewController (new UINavigationController(processing), true, null);
		}

		public void OrderCompleted ()
		{
			_navigation.PopToRootViewController (true);
		}
		BasketButton _button;
		public UIBarButtonItem CreateBasketButton ()
		{
			if (_button == null) {
				_button = new BasketButton () {
					Frame = new RectangleF (0, 0, 44, 44),
				};
				_button.TouchUpInside += (sender, args) => ShowBasket ();
			}
			_button.ItemsCount = WebService.Shared.CurrentOrder.Products.Count;
			return new UIBarButtonItem (_button);
		}
		public void UpdateProductsCount()
		{
			_button.UpdateItemsCount(WebService.Shared.CurrentOrder.Products.Count);
		}
	}
}
