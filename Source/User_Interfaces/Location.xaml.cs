
using GuideTravel.Models;
using Source.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using VietTravel;
using VietTravel.DBModels;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using winsdkfb;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Source.User_Interfaces
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Location : Page
    {
        short locationId;
        DispatcherTimer timer = null;
        List<string> listImage = new List<string>();
        LocationDataContext locationDC;
        ReviewViewModel reviewDC;
        int count = 0;

        public Location()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            List<string> temp = locationDC.ListImage;
            for (int i = 0; i < temp.Count; i++)
            {
                listImage.Add(temp[i]);
            }

            if (listImage.Count == 0)
                img.Source = new BitmapImage(new Uri("ms-appx://"));
            else
                img.Source = new BitmapImage(new Uri("ms-appx://" + listImage[0]));

            //Add star to listStar
            listStar.Add(star0);
            listStar.Add(star1);
            listStar.Add(star2);
            listStar.Add(star3);
            listStar.Add(star4);

            FillColorStar(listStar, -1);

            //Load history review
            //Add star to listStar in user's review
            //List<TextBlock> listStarEachUser = new List<TextBlock>();
            //listStarEachUser.Add(star00);
            //listStarEachUser.Add(star11);
            //listStarEachUser.Add(star22);
            //listStarEachUser.Add(star33);
            //listStarEachUser.Add(star44);

            //FillColorStar(listStarEachUser, 2);

            //FBSession sess = FBSession.ActiveSession;
            //await sess.LoginAsync();

            //if (sess.LoggedIn)
            //{
            //    userProPic.UserId = sess.User.Id;
            //    userName.Text = sess.User.Name;
            //}
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.locationId = (short)e.Parameter;
            locationDC = new LocationDataContext(locationId);
            this.DataContext = locationDC;
            reviewDC = new ReviewViewModel(locationId);
            lvReview.DataContext = reviewDC;
            txtNumOfReview.DataContext = reviewDC;

            count = 0;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Tick += PlayListTimer_Tick;
            timer.Start();
        }

        private void PlayListTimer_Tick(object sender, object e)
        {
            if (listImage != null)
            {
                if (count == listImage.Count - 1)
                    count = -1;
                if (count < listImage.Count)
                {
                    count++;
                    ImageRotation();
                }
            }
        }

        private void ImageRotation()
        {
            img.Source = new BitmapImage(new Uri("ms-appx://" + listImage[count]));
            opaci.Begin();
        }

#region Feature Rating
        // <! Feature Rating
        List<TextBlock> listStar = new List<TextBlock>();
        Brush colorStar = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
        SolidColorBrush colorDefaultStar = new SolidColorBrush(Colors.Black);
        int numberStarCurrent;

        private void RefreshColorStar()
        {
            foreach (var star in listStar)
            {
                star.Foreground = colorDefaultStar;
            }
        }

        private string GetRatingName(int numCurStar)
        {
            switch (numCurStar)
            {
                case 0:
                    {
                        return "bad";
                    }
                case 1:
                    {
                        return "average";
                    }
                case 2:
                    {
                        return "good";
                    }
                case 3:
                    {
                        return "very good";
                    }
                case 4:
                    {
                        return "awesome";
                    }
                default:
                    {
                        return "";
                    };
            }
        }

        private void FillColorStar(List<TextBlock> list, int numCurStar)
        {
            RefreshColorStar();
            ratingStarName.Text = GetRatingName(numCurStar);
            for (int i = 0; i < numCurStar + 1; i++)
            {
                list[i].Foreground = colorStar;                
            }

            this.numberStarCurrent = numCurStar;
        }

        private void star0_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            FillColorStar(listStar, 0);
        }

        private void star1_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            FillColorStar(listStar, 1);
        }

        private void star2_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            FillColorStar(listStar, 2);
        }

        private void star3_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            FillColorStar(listStar, 3);
        }

        private void star4_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            FillColorStar(listStar, 4);
        }

        private async void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (this.numberStarCurrent > 0)
            {
                FBSession sess = FBSession.ActiveSession;
                await sess.LoginAsync();

                if (sess.LoggedIn)
                {
                    using (var db = new VietTravelDBContext())
                    {
                        db.APPRECIATIONs.Add(new APPRECIATION()
                        {
                            locationID = this.locationId,
                            userFBID = sess.User.Id,
                            userFBName = sess.User.Name,
                            appreciaID = (short)(db.APPRECIATIONs.Count() + 1),
                            numStar = (short)this.numberStarCurrent,
                            title = txtTitle.Text,
                            content = txtReview.Text,
                            date = DateTime.Now
                        });

                        db.SaveChanges();
                    }

                    txtReview.Text = txtTitle.Text = "";
                    this.FillColorStar(listStar, -1);

                    //Refresh DataContext
                    lvReview.DataContext = null;
                    lvReview.DataContext = reviewDC;
                    txtNumOfReview.DataContext = null;
                    txtNumOfReview.DataContext = reviewDC;
                }
                else
                {
                    ContentDialog loginNotifiDialog = new ContentDialog() { Title = "Please! Login Facebook to submit rating", Content = "Many thanks for your support", SecondaryButtonText = "OK" };
                    await loginNotifiDialog.ShowAsync();
                }
            }
            else
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            }
        }
        //Feature Rating !>
#endregion Feature Rating

#region Transform MAP
        private void mapImage_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            double dblDelta_Scroll = -1 * e.GetCurrentPoint(mapImage).Properties.MouseWheelDelta;

            dblDelta_Scroll = (dblDelta_Scroll > 0) ? 1.2 : 0.8;

            double new_ScaleX = this.mapImage_Transform.ScaleX * dblDelta_Scroll;
            double new_ScaleY = this.mapImage_Transform.ScaleY * dblDelta_Scroll;

            this.mapImage_Transform.CenterX = e.GetCurrentPoint(mapImage).Position.X;
            this.mapImage_Transform.CenterY = e.GetCurrentPoint(mapImage).Position.Y;

            this.mapImage_Transform.ScaleX = new_ScaleX;
            this.mapImage_Transform.ScaleY = new_ScaleY;
        }

        private void mapImage_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            this.mapImage.Opacity = 0.4;
        }

        private void mapImage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            this.mapImage_Transform.TranslateX += e.Delta.Translation.X;
            this.mapImage_Transform.TranslateY += e.Delta.Translation.Y;
        }

        private void mapImage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            this.mapImage.Opacity = 1;
        }

        int countShowNotif = 0;
        private void scrollViewer_MAP_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (countShowNotif < 1)
            {
                FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
                countShowNotif++;
            }
        }

        private void scrollViewer_MAP_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(User_Interfaces.MapViewPage), this.locationDC.Address);
        }

        #endregion Transform MAP

    }
}

