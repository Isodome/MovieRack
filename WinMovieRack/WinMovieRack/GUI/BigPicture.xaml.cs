using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
namespace WinMovieRack.GUI
{
    /// <summary>
    /// Interaction logic for BigPicture.xaml
    /// </summary>
    public partial class BigPicture : Window
    {
		double posx;
		double posy;
		double orgHeight;
		double orgWidth;

        public BigPicture()
        {
            InitializeComponent();
            this.MaxHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
			this.Loaded += (object sender, RoutedEventArgs e) => {
				fadeIn();
			};
        }
		public void setOrigin(double height, double width, double posx, double posy) {
			this.orgWidth = width;
			this.orgHeight = height;
			this.posx = posx;
			this.posy = posy;
		}

		private void fadeIn() {
			
			this.Height = orgHeight;
			this.Width = orgWidth;
			this.Left = posx;
			this.Top = posy;
			
			Storyboard sb = new Storyboard();
			DoubleAnimation heightAnim = new DoubleAnimation();
			DoubleAnimation widthAnim = new DoubleAnimation();
			DoubleAnimation posXAnim = new DoubleAnimation();
			DoubleAnimation posYAnim = new DoubleAnimation();

			Duration dur1sec = new Duration(TimeSpan.FromMilliseconds(500));

			heightAnim.Duration = dur1sec;
			widthAnim.Duration = dur1sec;
			posXAnim.Duration = dur1sec;
			posYAnim.Duration = dur1sec;

			sb.Children.Add(heightAnim);
			sb.Children.Add(widthAnim);
			sb.Children.Add(posXAnim);
			sb.Children.Add(posYAnim);

			Storyboard.SetTarget(heightAnim, this);
			Storyboard.SetTarget(widthAnim, this);
			Storyboard.SetTarget(posXAnim, this);
			Storyboard.SetTarget(posYAnim, this);


			Storyboard.SetTargetProperty(heightAnim, new PropertyPath("(Canvas.Height)"));
			Storyboard.SetTargetProperty(widthAnim, new PropertyPath("(Canvas.Width)"));
			Storyboard.SetTargetProperty(posXAnim, new PropertyPath("(Canvas.Left)"));
			Storyboard.SetTargetProperty(posYAnim, new PropertyPath("(Canvas.Top)"));

			if ((double)bigPicture.Source.Width <= System.Windows.SystemParameters.PrimaryScreenWidth && (double)bigPicture.Source.Height <= System.Windows.SystemParameters.PrimaryScreenHeight) {
				widthAnim.To = (double)(bigPicture.Source as BitmapSource).PixelWidth;
				heightAnim.To = (double)(bigPicture.Source as BitmapSource).PixelHeight;
			
			} else {
				double picRatio = (double)bigPicture.Source.Width / (double)bigPicture.Source.Height;
				double screenRatio = System.Windows.SystemParameters.PrimaryScreenWidth / System.Windows.SystemParameters.PrimaryScreenHeight;
				if (picRatio > screenRatio) {
					widthAnim.To = System.Windows.SystemParameters.PrimaryScreenWidth;
					heightAnim.To = System.Windows.SystemParameters.PrimaryScreenWidth / bigPicture.Source.Width * bigPicture.Source.Height;
				} else {
					heightAnim.To = System.Windows.SystemParameters.PrimaryScreenHeight;
					widthAnim.To = System.Windows.SystemParameters.PrimaryScreenHeight / bigPicture.Source.Height * bigPicture.Source.Width;
				}
			}
			 
			posYAnim.To = (System.Windows.SystemParameters.PrimaryScreenHeight - heightAnim.To) /2;
			posXAnim.To = (System.Windows.SystemParameters.PrimaryScreenWidth - widthAnim.To) / 2;

			sb.Begin();
		}

        private void bigPicture_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bigPicture_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0.5;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void buttonGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 1;

        }

        private void buttonGrid_MouseLeave(object sender, MouseEventArgs e)
        {
           buttonGrid.Opacity = 0;
        }

        private void fadeOut() {   
			Storyboard sb = new Storyboard();
			DoubleAnimation heightAnim = new DoubleAnimation();
			DoubleAnimation widthAnim = new DoubleAnimation();
			DoubleAnimation posXAnim = new DoubleAnimation();
			DoubleAnimation posYAnim = new DoubleAnimation();
			DoubleAnimation opacityAnim = new DoubleAnimation();

			Duration duration = new Duration(TimeSpan.FromMilliseconds(500));
			Duration duration2 = new Duration(TimeSpan.FromMilliseconds(250));

			heightAnim.Duration = duration;
			widthAnim.Duration = duration;
			posXAnim.Duration = duration;
			posYAnim.Duration = duration;
			opacityAnim.Duration = duration2;

			sb.Children.Add(heightAnim);
			sb.Children.Add(widthAnim);
			sb.Children.Add(posXAnim);
			sb.Children.Add(posYAnim);
			//sb.Children.Add(opacityAnim);

			Storyboard.SetTarget(heightAnim, this);
			Storyboard.SetTarget(widthAnim, this);
			Storyboard.SetTarget(posXAnim, this);
			Storyboard.SetTarget(posYAnim, this);
			Storyboard.SetTarget(opacityAnim, this);


			Storyboard.SetTargetProperty(heightAnim, new PropertyPath("(Canvas.Height)"));
			Storyboard.SetTargetProperty(widthAnim, new PropertyPath("(Canvas.Width)"));
			Storyboard.SetTargetProperty(posXAnim, new PropertyPath("(Canvas.Left)"));
			Storyboard.SetTargetProperty(posYAnim, new PropertyPath("(Canvas.Top)"));
			Storyboard.SetTargetProperty(opacityAnim, new PropertyPath("(Canvas.Opacity)"));



			heightAnim.To = this.orgHeight;
			widthAnim.To = this.orgWidth;
			posYAnim.To = posy;
			posXAnim.To = posx;
			opacityAnim.To = 0.1;

			sb.Completed += closeThisWindow;
			sb.Begin();

        }

		private void closeThisWindow(object sender, EventArgs e) {
			this.Close();
		}

        private void saveButton_MouseEnter(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 1;
        }

        private void saveButton_MouseLeave(object sender, MouseEventArgs e)
        {
            buttonGrid.Opacity = 0;
        }

		private void bigPicture_MouseUp(object sender, MouseButtonEventArgs e) {
			fadeOut();
		}
		
    }
}
