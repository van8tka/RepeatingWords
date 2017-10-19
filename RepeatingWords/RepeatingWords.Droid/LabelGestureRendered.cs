using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;



[assembly: ExportRenderer(typeof(RepeatingWords.LabelGestureLongPress), typeof(RepeatingWords.Droid.LabelGestureRendered))]
namespace RepeatingWords.Droid
{
    public class LabelGestureRendered : LabelRenderer
    {

        private readonly LabelGestureListener _listener;
        private readonly GestureDetector _detector;


        public LabelGestureRendered()
        {
            _listener = new LabelGestureListener();
            _detector = new GestureDetector(_listener);
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement == null)
            {
                this.GenericMotion -= HandleGenericMotion;
                this.Touch -= HandleTouch;
            }
            if (e.OldElement == null)
            {
                this.GenericMotion += HandleGenericMotion;
                this.Touch += HandleTouch;
            }
        }
        void HandleTouch(object sender, TouchEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }
        void HandleGenericMotion(object sender, GenericMotionEventArgs e)
        {
            _detector.OnTouchEvent(e.Event);
        }
    }






}