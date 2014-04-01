using System.Windows;
using GalaSoft.MvvmLight;
using Pomodoro.Model;
using Pomodoro.ViewModel;

namespace Pomodoro
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		MainViewModel _vm;

		/// <summary>
		/// Initializes a new instance of the MainWindow class.
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			IClock clock = new Clock();
			_vm = new MainViewModel(clock);
			this.DataContext = _vm;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			_vm.Start();
		}
	}
}