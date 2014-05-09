using System.Drawing;
using MonoTouch.UIKit;

namespace XamarinStore
{
	public class TopAlignedImageView : UIView
	{
		SizeF _origionalSize;
		public UIImage Image
		{
			get { return image; }
			set
			{
				_origionalSize = value == null ? SizeF.Empty : value.Size;
				ImageView.Image = image = value;
				LayoutSubviews ();
			}
		}

	    readonly UIImageView ImageView;
		UIImage image;
	    readonly UIActivityIndicatorView progress;

		public TopAlignedImageView()
		{
			ClipsToBounds = true;
			ImageView = new UIImageView();
			this.AddSubview(ImageView);

			AddSubview (progress = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge));
			TranslatesAutoresizingMaskIntoConstraints = false;
		}
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			progress.Center = Center;
			if (_origionalSize == SizeF.Empty) {
				return;
			}
			var frame = Bounds;
			var scale = frame.Width/_origionalSize.Width ;
			frame.Height = _origionalSize.Height * scale;
			ImageView.Frame = frame;
		}
		public async void LoadUrl(string url)
		{
			if (string.IsNullOrEmpty (url))
				return;
			var t = FileCache.Download (url);
			if (t.IsCompleted) {
				Image = UIImage.FromFile(t.Result);
				return;
			}
			progress.StartAnimating ();
			var image = UIImage.FromFile(await t);

			Animate (.3, 
				() => Image = image,
				() => progress.StopAnimating ());
		}

	
	}
}
