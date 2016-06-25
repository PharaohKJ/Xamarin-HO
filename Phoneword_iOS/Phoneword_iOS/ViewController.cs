using System;
using Foundation;
using UIKit;
using System.Collections.Generic;

namespace Phoneword_iOS
{
	public partial class ViewController : UIViewController
	{
		// translatedNumber を ViewDidLoad()から移動します
		string translatedNumber = "";
		public List<string> PhoneNumbers { get; set; }

		protected ViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
			// Call History 画面用に電話番号の List を初期化します
			PhoneNumbers = new List<string>();
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Perform any additional setup after loading the view, typically from a nib.
			TranslateButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				// PhoneTranslator.cs を使用してテキストから電話番号に変換します 
				translatedNumber = Core.PhonewordTranslator.ToNumber(PhoneNumberText.Text); // TextField がタップされたらキーボードを Dismiss します
				PhoneNumberText.ResignFirstResponder();
				if (translatedNumber == "")
				{
					CallButton.SetTitle("Call", UIControlState.Normal); CallButton.Enabled = false;
				}
				else
				{
					CallButton.SetTitle("Call " + translatedNumber, UIControlState.Normal); CallButton.Enabled = true;
				}	
			};
			CallButton.TouchUpInside += (object sender, EventArgs e) =>
			{
				// 変換した電話番号を PhoneNumbers に追加します
				PhoneNumbers.Add(translatedNumber);

				var url = new NSUrl("tel:" + translatedNumber);
				// 標準の電話アプリを呼び出すために tel:のプリフィックスで URL ハンドラーを使用します
				// できない場合は AlertView を呼び出します。
				if (!UIApplication.SharedApplication.OpenUrl(url))
				{
					var alert = UIAlertController.Create(
						"Not supported", "Scheme 'tel:' is not supported on this device", 
						UIAlertControllerStyle.Alert
						);
					alert.AddAction(UIAlertAction.Create(
						"Ok", 
						UIAlertActionStyle.Default, 
						null
						));
                	PresentViewController(alert, true, null);
				}
			};
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);
			// set the View Controller that’s powering the screen we’re
			// transitioning to
			var callHistoryContoller = segue.DestinationViewController as CallHistoryController;
			//set the Table View Controller’s list of phone numbers to the // list of dialed phone numbers
			if (callHistoryContoller != null)
			{
				callHistoryContoller.PhoneNumbers = PhoneNumbers;
			}
		}
	}
}

