using System;
#if __UNIFIED__
using UIKit;

#else
using MonoTouch.UIKit;
#endif

namespace Xamarin.Forms.Platform.iOS
{
	internal class ModalWrapper : UIViewController
	{
		IVisualElementRenderer _modal;

		internal ModalWrapper(IVisualElementRenderer modal)
		{
			_modal = modal;

			View.BackgroundColor = UIColor.White;
			View.AddSubview(modal.ViewController.View);
			AddChildViewController(modal.ViewController);

			modal.ViewController.DidMoveToParentViewController(this);
		}
#if !__TVOS__
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
			return UIInterfaceOrientationMask.All;
		}
#endif
		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();
			if (_modal != null)
				_modal.SetElementSize(new Size(View.Bounds.Width, View.Bounds.Height));
		}

		public override void ViewWillAppear(bool animated)
		{
			View.BackgroundColor = UIColor.White;
			base.ViewWillAppear(animated);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
				_modal = null;
			base.Dispose(disposing);
		}
	}

	internal class PlatformRenderer : UIViewController
	{
		internal PlatformRenderer(Platform platform)
		{
			Platform = platform;
		}

		public Platform Platform { get; set; }
#if !__TVOS__
		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
		{
			return UIInterfaceOrientationMask.All;
		}
#endif
		public override void ViewDidAppear(bool animated)
		{
			Platform.DidAppear();
			base.ViewDidAppear(animated);
		}

		public override void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();
			Platform.LayoutSubviews();
		}

		public override void ViewWillAppear(bool animated)
		{
			View.BackgroundColor = UIColor.White;
			Platform.WillAppear();
			base.ViewWillAppear(animated);
		}
	}
}